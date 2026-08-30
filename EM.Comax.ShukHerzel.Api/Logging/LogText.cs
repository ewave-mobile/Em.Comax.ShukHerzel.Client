namespace EM.Comax.Api.Logging;

public static class LogText
{
    public const int SourceNameMaxLength = 50;
    public const int ErrorRequestMaxLength = 2500;
    public const int ExceptionMessageMaxLength = 1000;
    public const int InnerExceptionMaxLength = 2500;

    public static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
