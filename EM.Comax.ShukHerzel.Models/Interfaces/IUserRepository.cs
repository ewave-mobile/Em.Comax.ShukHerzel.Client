using EM.Comax.ShukHerzel.Models.Models;

namespace EM.Comax.ShukHerzel.Models.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task UpdateTempCodeAsync(int userId, string? tempCode, CancellationToken cancellationToken = default);
}
