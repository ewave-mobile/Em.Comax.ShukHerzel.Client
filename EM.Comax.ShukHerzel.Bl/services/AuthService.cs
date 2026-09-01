using System.Security.Cryptography;
using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using Microsoft.Extensions.Options;

namespace EM.Comax.ShukHerzel.Bl.services;

public class AuthService : IAuthService
{
    private const string LogSource = "SMS_OTP_AUTH";

    private readonly IUserRepository _userRepository;
    private readonly IMaskyooSmsService _smsService;
    private readonly SmsSettings _smsSettings;
    private readonly IDatabaseLogger _databaseLogger;

    public AuthService(
        IUserRepository userRepository,
        IMaskyooSmsService smsService,
        IOptions<SmsSettings> smsSettings,
        IDatabaseLogger databaseLogger)
    {
        _userRepository = userRepository;
        _smsService = smsService;
        _smsSettings = smsSettings.Value;
        _databaseLogger = databaseLogger;
    }

    public async Task<LoginResult> LoginAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            return new LoginResult(false, "יש להזין שם משתמש וסיסמה");
        }

        var user = await _userRepository.GetByUserNameAsync(userName.Trim(), cancellationToken);
        if (user is null || !string.Equals(user.Password, password, StringComparison.Ordinal))
        {
            return new LoginResult(false, "שם משתמש או סיסמה שגויים");
        }

        if (string.IsNullOrWhiteSpace(user.CellPhone))
        {
            return new LoginResult(false, "לא מוגדר מספר נייד למשתמש");
        }

        var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        await _userRepository.UpdateTempCodeAsync(user.Id, code, cancellationToken);

        var message = BuildOtpMessage(code);
        var phoneSuffix = user.CellPhone.Length >= 4 ? user.CellPhone[^4..] : "****";
        var otpOrigin = string.IsNullOrWhiteSpace(_smsSettings.OtpOrigin) ? "(none)" : _smsSettings.OtpOrigin;
        var request = $"userName={user.UserName}, userId={user.Id}, phone=***{phoneSuffix}, otpOrigin={otpOrigin}";

        await LogInfoAsync(request, "Sending OTP SMS", "Information");

        // OTP must go as SMS text — voice dialer prefixes break SSL / are unsuitable for codes.
        var sent = await _smsService.SendAsync(message, user.CellPhone, cancellationToken, smsOnly: true);
        if (!sent)
        {
            await LogErrorAsync(request, "FAILED: SMS provider returned failure");
            return new LoginResult(false, "שליחת קוד האימות נכשלה");
        }

        await LogInfoAsync(request, "OK: SMS provider returned success", "OK");

        return new LoginResult(true);
    }

    public async Task<VerifyResult> VerifyAsync(string userName, string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code))
        {
            return new VerifyResult(false, ErrorMessage: "יש להזין קוד אימות");
        }

        var user = await _userRepository.GetByUserNameAsync(userName.Trim(), cancellationToken);
        if (user is null ||
            string.IsNullOrWhiteSpace(user.TempCode) ||
            !string.Equals(user.TempCode.Trim(), code.Trim(), StringComparison.Ordinal))
        {
            return new VerifyResult(false, ErrorMessage: "קוד אימות שגוי");
        }

        await _userRepository.UpdateTempCodeAsync(user.Id, null, cancellationToken);
        return new VerifyResult(true, user);
    }

    private string BuildOtpMessage(string code)
    {
        var message = $"קוד האימות שלך הוא: {code}";
        var origin = (_smsSettings.OtpOrigin ?? string.Empty).Trim().TrimEnd('/');

        if (origin.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            origin = origin["https://".Length..];
        }
        else if (origin.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        {
            origin = origin["http://".Length..];
        }

        if (!string.IsNullOrWhiteSpace(origin))
        {
            // Web OTP format required by Chrome on Android.
            message += $"\n\n@{origin} #{code}";
        }

        return message;
    }

    private async Task LogInfoAsync(string request, string response, string status)
    {
        try
        {
            await _databaseLogger.LogTraceAsync(LogSource, request, response, status);
        }
        catch
        {
            // Never fail login flow because logging failed.
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
            // Never fail login flow because logging failed.
        }
    }
}
