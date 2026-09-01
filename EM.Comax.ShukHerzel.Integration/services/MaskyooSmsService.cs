using System.Dynamic;
using System.Net;
using System.Xml.Linq;
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EM.Comax.ShukHerzel.Integration.services;

public class MaskyooSmsService : IMaskyooSmsService
{
    private const string LogSource = "MASKYOO_SMS";

    private static readonly string[] VoiceMessagePhones =
    {
        "05532", "05567", "05832", "05341", "05331",
        "05485", "05484", "05276", "05271", "05041"
    };

    private readonly HttpClient _httpClient;
    private readonly SmsSettings _settings;
    private readonly IDatabaseLogger _databaseLogger;

    public MaskyooSmsService(
        HttpClient httpClient,
        IOptions<SmsSettings> settings,
        IDatabaseLogger databaseLogger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _databaseLogger = databaseLogger;
    }

    public async Task<bool> SendAsync(
        string message,
        string destination,
        CancellationToken cancellationToken = default,
        bool smsOnly = false)
    {
        var dest = NormalizeDestination(destination);
        if (string.IsNullOrWhiteSpace(dest))
        {
            await LogErrorAsync(
                $"destination={destination}",
                "FAILED: empty destination after normalization");
            return false;
        }

        var maskedDest = MaskPhone(dest);
        await LogInfoAsync(
            $"destination={maskedDest}, smsOnly={smsOnly}, sender={_settings.Sender}, hasSmsUrl={!string.IsNullOrWhiteSpace(_settings.SmsUrl)}",
            "STARTED",
            "Information");

        try
        {
            var isVoiceMessage = !smsOnly &&
                VoiceMessagePhones.Any(s => dest.StartsWith(s, StringComparison.Ordinal));

            if (isVoiceMessage)
            {
                return await SendVoiceAsync(message, dest, maskedDest, cancellationToken);
            }

            return await SendSmsAsync(message, dest, maskedDest, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            await LogErrorAsync($"destination={maskedDest}", $"FAILED (HTTP): {ex.Message}");
            return false;
        }
        catch (TaskCanceledException ex)
        {
            await LogErrorAsync($"destination={maskedDest}", $"FAILED (timeout): {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            await LogErrorAsync($"destination={maskedDest}", $"FAILED: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> SendVoiceAsync(
        string message,
        string dest,
        string maskedDest,
        CancellationToken cancellationToken)
    {
        var messageEncoded = Uri.EscapeDataString(message);
        var url =
            $"https://dialer2.maskyoo.com/dialer_logisteam_tts/dialer_tts_ws.php" +
            $"?service=makecall&retry=2&waittime=900&destination={dest}&playfile1={messageEncoded}";

        using var httpResponse = await _httpClient.PostAsync(url, null, cancellationToken);
        var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        if (httpResponse.StatusCode == HttpStatusCode.OK && httpResponse.IsSuccessStatusCode)
        {
            await LogInfoAsync(
                $"destination={maskedDest}, status={(int)httpResponse.StatusCode}",
                Truncate(responseBody),
                "OK");
            return true;
        }

        await LogErrorAsync(
            $"destination={maskedDest}, status={(int)httpResponse.StatusCode}",
            $"FAILED (voice): {Truncate(responseBody)}");
        return false;
    }

    private async Task<bool> SendSmsAsync(
        string message,
        string dest,
        string maskedDest,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_settings.SmsUrl))
        {
            await LogErrorAsync($"destination={maskedDest}", "FAILED: SmsSettings.SmsUrl is empty");
            return false;
        }

        var requestUrl =
            _settings.SmsUrl +
            "dest=" + dest +
            "&sender=" + Uri.EscapeDataString(_settings.Sender ?? string.Empty) +
            "&username=" + Uri.EscapeDataString(_settings.UserName ?? string.Empty) +
            "&password=" + Uri.EscapeDataString(_settings.Password ?? string.Empty) +
            "&message=" + Uri.EscapeDataString(message);

        await LogInfoAsync(
            $"destination={maskedDest}, url={RedactPassword(requestUrl)}",
            "HTTP request",
            "Information");

        using var httpResponse = await _httpClient.GetAsync(requestUrl, cancellationToken);
        var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        if (httpResponse.StatusCode != HttpStatusCode.OK || !httpResponse.IsSuccessStatusCode)
        {
            await LogErrorAsync(
                $"destination={maskedDest}, status={(int)httpResponse.StatusCode}",
                $"FAILED (HTTP): {Truncate(responseBody)}");
            return false;
        }

        try
        {
            var doc = XDocument.Parse(responseBody);
            var jsonText = JsonConvert.SerializeXNode(doc);
            dynamic? dynamicResponse = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

            var messageIdText = Convert.ToString((object?)dynamicResponse?.SMS_Service?.sms_recipients?.sms_0?.Message_id);
            if (!string.IsNullOrWhiteSpace(messageIdText) && long.TryParse(messageIdText, out long messageId))
            {
                await LogInfoAsync(
                    $"destination={maskedDest}, messageId={messageId}",
                    Truncate(responseBody),
                    "OK");
                return true;
            }

            var providerStatus = Convert.ToString((object?)dynamicResponse?.SMS_Service?.sms_recipients?.sms_0?.Status) ?? string.Empty;
            var providerDescription = Convert.ToString((object?)dynamicResponse?.SMS_Service?.sms_recipients?.sms_0?.description) ?? string.Empty;

            await LogErrorAsync(
                $"destination={maskedDest}, providerStatus={providerStatus}, providerDescription={providerDescription}",
                $"FAILED (provider): {Truncate(responseBody)}");
            return false;
        }
        catch (Exception ex)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                await LogInfoAsync(
                    $"destination={maskedDest}",
                    $"OK (unparsed HTTP 200): {Truncate(responseBody)}",
                    "OK");
                return true;
            }

            await LogErrorAsync(
                $"destination={maskedDest}",
                $"FAILED (parse): {ex.Message}; response={Truncate(responseBody)}");
            return false;
        }
    }

    private async Task LogInfoAsync(string request, string response, string status)
    {
        try
        {
            await _databaseLogger.LogTraceAsync(LogSource, request, response, status);
        }
        catch
        {
            // Never fail SMS flow because logging failed.
        }
    }

    private async Task LogErrorAsync(string request, string message)
    {
        try
        {
            await _databaseLogger.LogToErrorLogAsync(LogSource, request, message);
        }
        catch
        {
            // Never fail SMS flow because logging failed.
        }
    }

    private static string NormalizeDestination(string destination)
    {
        var dest = (destination ?? string.Empty).Trim();
        if (dest.StartsWith("+972", StringComparison.Ordinal))
        {
            dest = "0" + dest[4..];
        }

        return dest;
    }

    private static string MaskPhone(string phone)
    {
        if (string.IsNullOrEmpty(phone) || phone.Length <= 4)
        {
            return "***";
        }

        return "***" + phone[^4..];
    }

    private static string RedactPassword(string url)
    {
        const string marker = "password=";
        var index = url.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
        {
            return url;
        }

        var start = index + marker.Length;
        var end = url.IndexOf('&', start);
        if (end < 0)
        {
            return url[..start] + "***";
        }

        return url[..start] + "***" + url[end..];
    }

    private static string Truncate(string? value, int maxLength = 500)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }
}
