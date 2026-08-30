namespace EM.Comax.ShukHerzel.Models.CustomModels;

public class SmsSettings
{
    public string SmsUrl { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// Host for Web OTP SMS format (no scheme), e.g. localhost:4300 or app.example.com.
    /// When empty, the origin line is omitted and autofill will not work on Android Chrome.
    /// </summary>
    public string OtpOrigin { get; set; } = string.Empty;
}
