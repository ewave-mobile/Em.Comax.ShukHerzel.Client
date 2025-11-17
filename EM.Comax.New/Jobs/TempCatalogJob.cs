
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
    public class TempCatalogJob : IJob
    {
        private readonly IAllItemsNewService _allItemsNewService;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IConfiguration _configuration;
        private readonly IBranchRepository _branchRepository;

        public TempCatalogJob( IDatabaseLogger databaseLogger, IConfiguration configuration, IBranchRepository branchRepository, IAllItemsNewService allItemsNewService)
        {
            _databaseLogger = databaseLogger;
            _configuration = configuration;
            _branchRepository = branchRepository;
            _allItemsNewService = allItemsNewService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
           await _databaseLogger.LogServiceActionAsync("TempCatalogJob started.");

            var progress = new Progress<string>(async message =>
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
                foreach( var branch in branches)
                {
                    await _allItemsNewService.InsertCatalogAsync(branch, null, progress);
                }
            }
            catch (Exception ex)
            {
               // Log.Error(ex, "Error occurred in TempCatalogJob.");
                await _databaseLogger.LogErrorAsync("TempCatalogJob", "Error during Temp Catalog synchronization", ex);
            }

         //   Log.Information("TempCatalogJob completed.");
        }
    }
}
