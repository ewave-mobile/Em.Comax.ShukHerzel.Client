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
    public class OperativeJob : IJob
    {
        private readonly IOperativeService _operativeService;
        private readonly IDatabaseLogger _databaseLogger;

        public OperativeJob(IOperativeService operativeService, IDatabaseLogger databaseLogger)
        {
            _operativeService = operativeService;
            _databaseLogger = databaseLogger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
           // Log.Information("OperativeJob started.");

            var progress = new Progress<string>(message =>
            {
                //Log.Information(message);
            });

            try
            {
                await _operativeService.SyncAllItemsAndPromotionsAsync(progress, CancellationToken.None);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "Error occurred in OperativeJob.");
                await _databaseLogger.LogErrorAsync("OperativeJob", "Error during Operative synchronization", ex);
            }

           // Log.Information("OperativeJob completed.");
        }
    }
}
