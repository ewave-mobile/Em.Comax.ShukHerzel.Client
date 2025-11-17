// QuartzExtensions.cs
using Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EM.Comax.New.WorkerService
{
    public static class QuartzExtensions
    {
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration configuration)
            where T : IJob
        {
            // Get job configurations from appsettings.json
            var jobConfigSection = configuration.GetSection("QuartzJobs").GetSection(typeof(T).Name);
            var jobConfig = jobConfigSection.Get<JobConfig>();

            if (jobConfig == null)
            {
                throw new Exception($"Configuration for job {typeof(T).Name} not found.");
            }

            // Register the job
            quartz.AddJob<T>(opts => opts.WithIdentity(typeof(T).Name));

            // Register the trigger
            quartz.AddTrigger(opts => opts
                .ForJob(typeof(T).Name)
                .WithIdentity($"{typeof(T).Name}-trigger")
                .WithCronSchedule(jobConfig.CronExpression));
        }
    }

    // JobConfig.cs
    public class JobConfig
    {
        public string CronExpression { get; set; }
    }
}
