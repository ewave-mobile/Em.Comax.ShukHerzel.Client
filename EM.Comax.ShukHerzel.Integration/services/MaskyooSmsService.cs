using System.Dynamic;
using System.Net;
using System.Xml.Linq;
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.CustomModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EM.Comax.ShukHerzel.Integration.services;

public class MaskyooSmsService : IMaskyooSmsService
{
    private static readonly string[] VoiceMessagePhones =
    {
        "05532", "05567", "05832", "05341", "05331",
        "05485", "05484", "05276", "05271", "05041"
    };

    private readonly HttpClient _httpClient;
    private readonly SmsSettings _settings;

    public MaskyooSmsService(HttpClient httpClient, IOptions<SmsSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
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
            return false;
        }

        try
        {
            var isVoiceMessage = !smsOnly &&
                VoiceMessagePhones.Any(s => dest.StartsWith(s, StringComparison.Ordinal));

            if (isVoiceMessage)
            {
                return await SendVoiceAsync(message, dest, cancellationToken);
            }

            return await SendSmsAsync(message, dest, cancellationToken);
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
    }

    private async Task<bool> SendVoiceAsync(string message, string dest, CancellationToken cancellationToken)
    {
        var messageEncoded = Uri.EscapeDataString(message);
        var url =
            $"https://dialer2.maskyoo.com/dialer_logisteam_tts/dialer_tts_ws.php" +
            $"?service=makecall&retry=2&waittime=900&destination={dest}&playfile1={messageEncoded}";

        using var httpResponse = await _httpClient.PostAsync(url, null, cancellationToken);
        _ = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        return httpResponse.StatusCode == HttpStatusCode.OK && httpResponse.IsSuccessStatusCode;
    }

    private async Task<bool> SendSmsAsync(string message, string dest, CancellationToken cancellationToken)
    {
        var requestUrl =
            _settings.SmsUrl +
            "dest=" + dest +
            "&sender=" + Uri.EscapeDataString(_settings.Sender ?? string.Empty) +
            "&username=" + Uri.EscapeDataString(_settings.UserName ?? string.Empty) +
            "&password=" + Uri.EscapeDataString(_settings.Password ?? string.Empty) +
            "&message=" + Uri.EscapeDataString(message);

        using var httpResponse = await _httpClient.GetAsync(requestUrl, cancellationToken);
        var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        if (httpResponse.StatusCode != HttpStatusCode.OK || !httpResponse.IsSuccessStatusCode)
        {
            return false;
        }

        try
        {
            var doc = XDocument.Parse(responseBody);
            var jsonText = JsonConvert.SerializeXNode(doc);
            dynamic? dynamicResponse = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

            if (dynamicResponse?.SMS_Service?.sms_recipients?.sms_0?.Message_id != null &&
                long.TryParse((string)dynamicResponse.SMS_Service.sms_recipients.sms_0.Message_id, out _))
            {
                return true;
            }

            return false;
        }
        catch
        {
            return httpResponse.IsSuccessStatusCode;
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
}
