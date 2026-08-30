using EM.Comax.ShukHerzel.Models.Models;

namespace EM.Comax.ShukHerzel.Bl.interfaces;

public interface IJwtTokenService
{
    (string Token, int ExpiresInSeconds) CreateToken(User user);
}
