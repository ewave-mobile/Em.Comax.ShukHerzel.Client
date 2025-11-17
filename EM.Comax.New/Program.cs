using EM.Comax.ShukHerzl.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using EM.Comax.New.WorkerService.Services;
using CliWrap;

namespace EM.Comax.New.WorkerService
{
    public class Program
    {
        // Define constants for service name and target installation directory
        private const string ServiceName = "EM.Comax.New.Service";
        private const string TargetDirectory = @"C:\Program Files (x86)\EwaveMobile\Em.Comax.New.installer1\";
        private const string ExecutableName = "EM.Comax.New.Service.exe";
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
                        //services.AddScoped<Service.Jobs.SyncItemsJob>();
                        services.AddScoped<Service.Jobs.TempCatalogJob>();
                       // services.AddScoped<Service.Jobs.PromotionJob>();
                        //services.AddScoped<Service.Jobs.OperativeJob>();
                        //services.AddScoped<Service.Jobs.MaintenanceJob>();
                        //services.AddScoped<Service.Jobs.PriceUpdateJob>();

                        // Register Quartz services
                        services.AddQuartz(q =>
                        {
                            q.UseMicrosoftDependencyInjectionJobFactory();
                            //q.AddJobAndTrigger<Service.Jobs.SyncItemsJob>(hostContext.Configuration);
                            q.AddJobAndTrigger<Service.Jobs.TempCatalogJob>(hostContext.Configuration);
                            //q.AddJobAndTrigger<Service.Jobs.PromotionJob>(hostContext.Configuration);
                            //q.AddJobAndTrigger<Service.Jobs.OperativeJob>(hostContext.Configuration);
                           // q.AddJobAndTrigger<Service.Jobs.MaintenanceJob>(hostContext.Configuration);
                           // q.AddJobAndTrigger<Service.Jobs.PriceUpdateJob>(hostContext.Configuration);
                        });

                        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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

                // If source != destination, copy the file
                if (!string.Equals(sourceExecutablePath, ExecutablePath, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Copying executable to: {ExecutablePath}");
                    File.Copy(sourceExecutablePath, ExecutablePath, overwrite: true);
                    Console.WriteLine($"Copied executable to: {ExecutablePath}");
                }
                else
                {
                    Console.WriteLine("Source and target paths are the same. Skipping copy.");
                }

                // Install the service using sc.exe via CliWrap
                Console.WriteLine("Creating service...");

                // Construct the command in a sc-friendly format:
                //   sc create EM.Comax.ShukHerzel.Service1
                //      binPath= "C:\Program Files (x86)\...\EM.Comax.ShukHerzel.Service.exe"
                //      start=auto
                //      DisplayName= "EM Comax ShukHerzel Service"
                //      Description= "Handles background tasks for ShukHerzel."
                var scCreateCommand = Cli.Wrap("sc")
                    .WithArguments(new[]
                    {
        "create",
        ServiceName,                      // e.g. "EM.Comax.ShukHerzel.Service1"
        "binPath= \"" + ExecutablePath + "\"",
        "start=auto",
        "DisplayName= \"EM.Comax.ShukHerzel.Service\""
                        // "Description= \"Handles background tasks for ShukHerzel.\""
                        // ... etc. if you want
                    });

                // Now execute it
                //write line of command to console
                Console.WriteLine(scCreateCommand.ToString());
                var createResult = await scCreateCommand.ExecuteAsync();
                Console.WriteLine($"sc create ExitCode: {createResult.ExitCode}");
                //  Console.WriteLine($"StdOut: {createResult}");
                //  Console.WriteLine($"StdErr: {createResult.StandardError}");

                if (createResult.ExitCode != 0)
                {
                    // If you want to bail out, do it here
                    Console.WriteLine("Service creation failed due to non-zero exit code.");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Service created successfully.");

                // Start the service
                Console.WriteLine("Starting service...");
                var startResult = await Cli.Wrap("sc")
                    .WithArguments(new[] { "start", ServiceName })
                    .ExecuteAsync();
                Console.WriteLine($"sc start ExitCode: {startResult.ExitCode}");
                //Console.WriteLine($"StdOut: {startResult.StandardOutput}");
                //Console.WriteLine($"StdErr: {startResult.StandardError}");

                if (startResult.ExitCode == 0)
                {
                    Console.WriteLine("Service started successfully.");
                }
                else
                {
                    Console.WriteLine("Service could not be started. Check the StdErr above for details.");
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Installation failed: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
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
                var stopResult = await Cli.Wrap("sc")
                    .WithArguments(new[] { "stop", ServiceName })
                    .ExecuteAsync();
                Console.WriteLine($"sc stop ExitCode: {stopResult.ExitCode}");
                //  Console.WriteLine($"StdOut: {stopResult.StandardOutput}");
                // Console.WriteLine($"StdErr: {stopResult.StandardError}");

                // Delete the service
                Console.WriteLine("Deleting service...");
                var deleteResult = await Cli.Wrap("sc")
                    .WithArguments(new[] { "delete", ServiceName })
                    .ExecuteAsync();
                Console.WriteLine($"sc delete ExitCode: {deleteResult.ExitCode}");
                //  Console.WriteLine($"StdOut: {deleteResult.StandardOutput}");
                // Console.WriteLine($"StdErr: {deleteResult.StandardError}");

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
