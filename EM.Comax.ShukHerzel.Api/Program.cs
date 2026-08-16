using EM.Comax.ShukHerzel.Api.Logging;
using EM.Comax.ShukHerzl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProjectServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebApp", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Allow GlobalExceptionHandler to re-read POST bodies for Log.ErrorLog.
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// CORS must run before other middleware that can short-circuit the response.
app.UseCors("WebApp");
app.UseExceptionHandler();
if (builder.Configuration.GetValue<bool>("CustomConfiguration:InsertTraceLog"))
{
    app.UseMiddleware<WebApiCustomMiddleware>();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
