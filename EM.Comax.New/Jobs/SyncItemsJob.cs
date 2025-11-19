using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Models.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.New.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class SyncItemsJob : IJob
    {
        private readonly IApiClientService _apiClientService;
        private readonly IDatabaseLogger _databaseLogger;

        public SyncItemsJob(IApiClientService apiClientService, IDatabaseLogger databaseLogger)
        {
            _apiClientService = apiClientService;
            _databaseLogger = databaseLogger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //Log.Information("SyncItemsJob started.");

            var progress = new Progress<string>(message =>
            {
                // Here, you can log progress messages or handle them as needed
                //Log.Information(message);
            });

            try
            {
                // Pass 'true' to log requests if needed
                await _apiClientService.SendItemsToEslAsync(true, progress, CancellationToken.None);
            }
            catch (Exception ex)
            {
               // Log.Error(ex, "Error occurred in SyncItemsJob.");
                await _databaseLogger.LogErrorAsync("SyncItemsJob", "Error during synchronization", ex);
            }

           // Log.Information("SyncItemsJob completed.");
        }
    }
}
