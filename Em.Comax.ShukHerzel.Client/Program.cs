using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzl.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Em.Comax.ShukHerzel.Client
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var configBuilder = new ConfigurationBuilder()
      .SetBasePath(AppContext.BaseDirectory) // This points to the current application directory
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = configBuilder.Build();


            var services = new ServiceCollection();

            services.AddProjectServices(configuration, isService: false);
           
            services.AddTransient<Form1>();
            var serviceProvider = services.BuildServiceProvider();
            var dbLogger = serviceProvider.GetRequiredService<IDatabaseLogger>();
            var mainForm = serviceProvider.GetRequiredService<Form1>();
            Application.ThreadException += (sender, args) =>
            {
                // The main UI thread exception
                // Synchronous logging (for a “last resort” scenario, it's usually acceptable)
                try
                {
                    dbLogger.LogErrorAsync("WinForms - ThreadException", null, args.Exception)
                        .GetAwaiter().GetResult();
                }
                catch
                {
                    // Fallback: you might log to a file or ignore if DB log fails
                }

                // Optionally show a user-friendly error message
                MessageBox.Show("An unexpected error occurred. Please contact support.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                // Non-UI thread or truly unhandled exception
                if (args.ExceptionObject is Exception ex)
                {
                    try
                    {
                        dbLogger.LogErrorAsync("WinForms - UnhandledException", null, ex)
                            .GetAwaiter().GetResult();
                    }
                    catch
                    {
                        // Fallback logging
                    }
                }

                // If you want to exit after a critical crash
                // Environment.Exit(1);
            };
            Application.Run(mainForm);
        }
    }
}