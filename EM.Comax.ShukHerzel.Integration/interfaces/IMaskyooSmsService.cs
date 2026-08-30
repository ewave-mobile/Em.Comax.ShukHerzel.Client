namespace EM.Comax.ShukHerzel.Integration.interfaces;

public interface IMaskyooSmsService
{
    /// <summary>
    /// Sends an SMS (or voice message for specific prefixes) via Maskyoo.
    /// </summary>
    /// <param name="smsOnly">When true, always send SMS and skip the voice dialer path.</param>
    /// <returns>True when the provider accepted the message.</returns>
    Task<bool> SendAsync(
        string message,
        string destination,
        CancellationToken cancellationToken = default,
        bool smsOnly = false);
}
