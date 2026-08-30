using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace EM.Comax.ShukHerzel.Dal.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ShukHerzelEntities context) : base(context)
    {
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.UserName == userName && (u.IsDeleted == null || u.IsDeleted == false),
                cancellationToken);
    }

    public async Task UpdateTempCodeAsync(int userId, string? tempCode, CancellationToken cancellationToken = default)
    {
        await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(u => u.TempCode, tempCode),
                cancellationToken);
    }
}
