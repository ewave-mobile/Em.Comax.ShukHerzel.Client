using EM.Comax.Api.Dtos;
using EM.Comax.ShukHerzel.Bl.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EM.Comax.ShukHerzl.Common;
namespace EM.Comax.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IAuthService authService, IJwtTokenService jwtTokenService)
    {
        _authService = authService;
        _jwtTokenService = jwtTokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        string encryptedPassword=null;
        if (request.Password != null) {

            encryptedPassword= EncryptManagement.PasswordEncrypt(request.Password);
        }
        var result = await _authService.LoginAsync(request.UserName, encryptedPassword, cancellationToken);
        if (!result.Success)
        {
            return Unauthorized(new { message = result.ErrorMessage });
        }

        return Ok(new { requiresOtp = true });
    }

    [AllowAnonymous]
    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyOtpRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.VerifyAsync(request.UserName, request.Code, cancellationToken);
        if (!result.Success || result.User is null)
        {
            return Unauthorized(new { message = result.ErrorMessage });
        }

        var (token, expiresIn) = _jwtTokenService.CreateToken(result.User);
        return Ok(new
        {
            accessToken = token,
            expiresIn
        });
    }
}
