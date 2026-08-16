using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EM.Comax.ShukHerzel.Api.Logging;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        IServiceScopeFactory scopeFactory,
        ILogger<GlobalExceptionHandler> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception for {Path}.", httpContext.Request.Path);

        await SaveErrorLogAsync(httpContext, exception);

        // Angular HttpClient treats non-2xx as error — keep 500 so existing subscribe({ error }) paths work.
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            new { message = exception.Message },
            cancellationToken);

        return true;
    }

    private async Task SaveErrorLogAsync(HttpContext httpContext, Exception exception)
    {
        try
        {
            // Separate scope — avoid the request DbContext that may already be in a failed state.
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ShukHerzelEntities>();

            dbContext.ErrorLogs.Add(new ErrorLog
            {
                SourceName = LogText.Truncate(ResolveSourceName(httpContext), LogText.SourceNameMaxLength),
                Request = LogText.Truncate(await ReadRequestAsync(httpContext), LogText.ErrorRequestMaxLength),
                ExceptionMessage = LogText.Truncate(exception.Message, LogText.ExceptionMessageMaxLength),
                InnerException = LogText.Truncate(
                    exception.InnerException?.ToString() ?? exception.StackTrace,
                    LogText.InnerExceptionMaxLength),
                CreateDate = DateTime.Now
            });

            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write error log for {Path}.", httpContext.Request.Path);
        }
    }

    private static string ResolveSourceName(HttpContext httpContext)
    {
        var descriptor = httpContext.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

        return descriptor is not null
            ? $"{descriptor.ControllerName}Controller::{descriptor.ActionName}"
            : $"{httpContext.Request.Method} {httpContext.Request.Path}";
    }

    private static async Task<string> ReadRequestAsync(HttpContext httpContext)
    {
        var url = $"{httpContext.Request.Method} {httpContext.Request.Path}{httpContext.Request.QueryString}";

        if (!httpContext.Request.Body.CanSeek || httpContext.Request.ContentLength is null or 0)
        {
            return url;
        }

        httpContext.Request.Body.Position = 0;
        using var reader = new StreamReader(httpContext.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();

        return $"{url} {body}";
    }
}
