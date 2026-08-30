namespace EM.Comax.Api.Dtos;

public class VerifyOtpRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
