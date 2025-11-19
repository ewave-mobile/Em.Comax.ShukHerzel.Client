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
    public class PromotionJob : IJob
    {
        private readonly IPromotionsService _promotionsService;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IConfiguration _configuration;
        private readonly IBranchRepository _branchRepository;

        public PromotionJob(IPromotionsService promotionsService, IDatabaseLogger databaseLogger, IConfiguration configuration, IBranchRepository branchRepository)
        {
            _promotionsService = promotionsService;
            _databaseLogger = databaseLogger;
            _configuration = configuration;
            _branchRepository = branchRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //Log.Information("PromotionJob started.");

            var progress = new Progress<string>(message =>
            {
               // Log.Information(message);
            });

            try
            {
                // Retrieve configuration for batch sizes or other settings if needed
                int batchSize = _configuration.GetValue<int>("BatchSettings:BatchSize", 1000);

                // Implement logic similar to the client
                // For example, always sends null update dates
                var branches = await _branchRepository.GetAllAsync();
                foreach (var branch in branches)
                {
                    await _promotionsService.InsertPromotionsAsync(branch, null, progress);
                }
                //await _promotionsService.InsertPromotionsAsync(null, null, progress);
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "Error occurred in PromotionJob.");
                await _databaseLogger.LogErrorAsync("PromotionJob", "Error during Promotions synchronization", ex);
            }

           // Log.Information("PromotionJob completed.");
        }
    }
}
