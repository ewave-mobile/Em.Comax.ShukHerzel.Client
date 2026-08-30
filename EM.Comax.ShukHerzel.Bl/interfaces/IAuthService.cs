using EM.Comax.ShukHerzel.Models.Models;

namespace EM.Comax.ShukHerzel.Bl.interfaces;

public record LoginResult(bool Success, string? ErrorMessage = null);

public record VerifyResult(bool Success, User? User = null, string? ErrorMessage = null);

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string userName, string password, CancellationToken cancellationToken = default);
    Task<VerifyResult> VerifyAsync(string userName, string code, CancellationToken cancellationToken = default);
}
