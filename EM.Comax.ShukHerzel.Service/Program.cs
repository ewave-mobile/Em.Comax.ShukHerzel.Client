using EM.Comax.ShukHerzel.Infrastructure;
using EM.Comax.ShukHerzel.Service;
using EM.Comax.ShukHerzel.WorkerService.Services;
using EM.Comax.ShukHerzl.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using System.Diagnostics;

namespace EM.Comax.ShukHerzel.WorkerService
{
    public class Program
    {
        // Define constants for service name and target installation directory
        private const string ServiceName = "EM.Comax.ShukHerzel.Service1";
        private const string TargetDirectory = @"C:\Program Files (x86)\EwaveMobile\Em.Comax.ShukHerzel.installer\";
        private const string ExecutableName = "EM.Comax.ShukHerzel.Service.exe";
        private static readonly string ExecutablePath = Path.Combine(TargetDirectory, ExecutableName);

        public static async Task Main(string[] args)
        {
            if (args is { Length: 1 })
            {
                string argument = args[0].ToLower();

                switch (argument)
                {
                    case "/install":
                        await InstallServiceAsync();
                        return;

                    case "/uninstall":
                        await UninstallServiceAsync();
                        return;

                    default:
                        Console.WriteLine("Unknown argument. Use /Install or /Uninstall.");
                        return;
                }
            }

            // Normal service execution
            try
            {
                Log.Information("Starting Worker Service");

                var host = Host.CreateDefaultBuilder(args)
                    .UseWindowsService(options => { options.ServiceName = ServiceName; })
                    .UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .WriteTo.File(Path.Combine(TargetDirectory, "logs", "worker_service.log"), rollingInterval: RollingInterval.Day);
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        // Register project services
                        services.AddProjectServices(hostContext.Configuration, isService: true);
                        services.AddScoped<Service.Jobs.SyncItemsJob>();
                        services.AddScoped<Service.Jobs.TempCatalogJob>();
                        services.AddScoped<Service.Jobs.PromotionJob>();
                        services.AddScoped<Service.Jobs.OperativeJob>();
                        services.AddScoped<Service.Jobs.MaintenanceJob>();
                        // Register Quartz services
                        services.AddQuartz(q =>
                        {
                            // Use Microsoft Dependency Injection Job Factory
                            q.UseMicrosoftDependencyInjectionJobFactory();

                            // Register Jobs and Triggers
                            q.AddJobAndTrigger<Service.Jobs.SyncItemsJob>(hostContext.Configuration);
                            q.AddJobAndTrigger<Service.Jobs.TempCatalogJob>(hostContext.Configuration);
                            q.AddJobAndTrigger<Service.Jobs.PromotionJob>(hostContext.Configuration);
                            q.AddJobAndTrigger<Service.Jobs.OperativeJob>(hostContext.Configuration);
                            q.AddJobAndTrigger<Service.Jobs.MaintenanceJob>(hostContext.Configuration);
                        });

                        // Add Quartz hosted service
                        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

                        // Register Service Lifecycle Logger
                        services.AddHostedService<ServiceLifecycleLogger>();
                    })
                    .Build();

                await host.RunAsync();
            }
            catch (AggregateException aggEx)
            {
                foreach (var ex in aggEx.InnerExceptions)
                {
                    Log.Fatal(ex, "An aggregate exception occurred during service startup.");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Worker Service terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Handles the installation of the Windows Service.
        /// </summary>
        private static async Task InstallServiceAsync()
        {
            try
            {
                Console.WriteLine("Starting installation...");

                // Ensure the target directory exists
                if (!Directory.Exists(TargetDirectory))
                {
                    Directory.CreateDirectory(TargetDirectory);
                    Console.WriteLine($"Created directory: {TargetDirectory}");
                }
                else
                {
                    Console.WriteLine($"Target directory already exists: {TargetDirectory}");
                }

                // Path to the currently running executable
                string sourceExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
                Console.WriteLine($"Source executable path: {sourceExecutablePath}");

                // Copy the executable to the target directory
                Console.WriteLine($"Copying executable to: {ExecutablePath}");
                File.Copy(sourceExecutablePath, ExecutablePath, overwrite: true);
                Console.WriteLine($"Copied executable to: {ExecutablePath}");

                // Install the service using sc.exe via CliWrap
                Console.WriteLine("Creating service...");
                await Cli.Wrap("sc")
                    .WithArguments(new[]
                    {
                        "create",
                        $"\"{ServiceName}\"",
                        $"binPath= \"{ExecutablePath}\"",
                        "start= auto",
                        $"DisplayName= \"EM Comax ShukHerzel Service\"",
                        $"Description= \"Handles background tasks for ShukHerzel.\""
                    })
                    .ExecuteAsync();
                Console.WriteLine("Service created successfully.");

                // Start the service
                Console.WriteLine("Starting service...");
                await Cli.Wrap("sc")
                    .WithArguments(new[] { "start", $"\"{ServiceName}\"" })
                    .ExecuteAsync();
                Console.WriteLine("Service started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Installation failed: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Handles the uninstallation of the Windows Service.
        /// </summary>
        private static async Task UninstallServiceAsync()
        {
            try
            {
                Console.WriteLine("Starting uninstallation...");

                // Stop the service
                Console.WriteLine("Stopping service...");
                await Cli.Wrap("sc")
                    .WithArguments(new[] { "stop", $"\"{ServiceName}\"" })
                    .ExecuteAsync();
                Console.WriteLine("Service stopped successfully.");

                // Delete the service
                Console.WriteLine("Deleting service...");
                await Cli.Wrap("sc")
                    .WithArguments(new[] { "delete", $"\"{ServiceName}\"" })
                    .ExecuteAsync();
                Console.WriteLine("Service deleted successfully.");

                // Remove the executable from the target directory
                if (File.Exists(ExecutablePath))
                {
                    Console.WriteLine($"Deleting executable from: {ExecutablePath}");
                    File.Delete(ExecutablePath);
                    Console.WriteLine("Executable deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Executable not found in the target directory.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Uninstallation failed: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
