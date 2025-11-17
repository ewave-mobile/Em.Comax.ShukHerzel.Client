using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Bl.services;
using EM.Comax.ShukHerzel.Dal.Repositories;
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Integration.services;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzl.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzl.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddProjectServices(
                this IServiceCollection services,
                IConfiguration configuration,
                bool isService = false)
        {
            // Register common services, repositories, etc.
            services.Configure<OutputSettings>(configuration.GetSection("OutputSettings"));
            services.AddDbContext<EM.Comax.ShukHerzel.Models.Models.ShukHerzelEntities>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ShukHerzelDb"));
            });

            services.AddScoped<IApiConfigService, ApiConfigService>();

        

               // services.AddHttpClient<IEslApiClient, EslApiClient>()
               //.ConfigureHttpClient((sp, client) =>
               //{
               //    var configSvc = sp.GetRequiredService<IApiConfigService>();
               //    var configEsl = configSvc.GetApiConfig("ESL"); // or "ESL_API" in DB

               //    client.BaseAddress = new Uri(configEsl.EslBaseUrl);
               //    // If ESL uses an API key
               //    if (!string.IsNullOrEmpty(configEsl.EslApiKey))
               //    {
               //        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", configEsl.EslApiKey);
               //    }
               //    client.Timeout = TimeSpan.FromMinutes(Constants.ESL_API_TIMEOUT_MINUTES);
               //    // Additional ESL headers or timeouts
               //    // client.Timeout = TimeSpan.FromSeconds(30);
               //});

               // services.AddHttpClient<IComaxApiClient, ComaxApiClient>()
               // .ConfigureHttpClient((sp, client) =>
               // {
               //     var configSvc = sp.GetRequiredService<IApiConfigService>();
               //     var configComax = configSvc.GetApiConfig("COMAX"); // or "COMAX_API" in DB

               //     client.BaseAddress = new Uri(configComax.ComaxBaseUrl);
               //     // Comax might not need an API key, or uses different headers
               //     client.Timeout = TimeSpan.FromMinutes(Constants.COMAX_API_TIMEOUT_MINUTES);
               // });

         
                services.AddHttpClient<IComaxApiClient, ComaxApiClient>();
                services.AddHttpClient<IEslApiClient, EslApiClient>();
           


            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IAllItemsService, AllItemsService>();
            services.AddScoped<IAllItemsNewService, AllItemsNewService>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IAllItemsRepository, AllItemsRepository>();
            services.AddScoped<IDatabaseLogger, DatabaseLogger>();
            services.AddScoped<IPromotionsService, PromotionsService>();
            services.AddScoped<IPromotionsRepository, PromotionsRepository>();
            services.AddScoped < IOperativeService, OperativeService>();
            services.AddScoped<IItemsRepository,ItemsRepository>();
            services.AddScoped<IApiClientService, ApiClientService>();
            services.AddScoped<IBadItemLogRepository, BadItemLogRepository>();
            services.AddScoped<IPriceUpdateService, PriceUpdateService>();
            services.AddScoped<IPriceUpdateRepository, PriceUpdateRepository>();
            services.AddScoped<ITrailingItemRepository, TrailingItemRepository>();
            services.AddScoped<IAllItemsNewRepository, AllItemsNewRepository>();
            // Possibly register Quartz only if we're in the service
            if (isService)
            {
                // Register your Quartz stuff here
                //services.AddQuartz(q =>
                //{
                //    // ...
                //});
                // For hosted service (if .NET Core worker service):
                // services.AddQuartzHostedService(...);
            }

            return services;
        }
    }
}
