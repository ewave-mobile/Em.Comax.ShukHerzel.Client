using EM.Comax.ShukHerzel.Models.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class MaintenanceJob : IJob
    {
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IBadItemLogRepository _badItemLogRepo;
        private readonly IAllItemsRepository _allItemsRepo;
        private readonly IItemsRepository _itemsRepo;
        private readonly IPromotionsRepository _promotionsRepository;
        private readonly IConfiguration _configuration;
        private readonly IPriceUpdateRepository _priceUpdateRepository;

        public MaintenanceJob(IDatabaseLogger databaseLogger,
                              IBadItemLogRepository badItemLogRepo,
                              IAllItemsRepository allItemsRepo,
                              IItemsRepository itemsRepo,
                              IConfiguration configuration,
                              IPromotionsRepository promotionsRepository,
                              IPriceUpdateRepository priceUpdateRepository)
        {
            _databaseLogger = databaseLogger;
            _badItemLogRepo = badItemLogRepo;
            _allItemsRepo = allItemsRepo;
            _itemsRepo = itemsRepo;
            _configuration = configuration;
            _promotionsRepository = promotionsRepository;
            _priceUpdateRepository = priceUpdateRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //Log.Information("MaintenanceJob started.");

            try
            {
                // Fetch maintenance settings from configuration
                var loggingRetentionDays = _configuration.GetValue<int>("MaintenanceSettings:LoggingRetentionDays", 30);
                var tempRetentionDays = _configuration.GetValue<int>("MaintenanceSettings:TempRetentionDays", 60);
                var operRetentionDays = _configuration.GetValue<int>("MaintenanceSettings:OperRetentionDays", 60);

                // 1. Delete old records from logging tables
                await DeleteOldLogsAsync(loggingRetentionDays);

                // 2. Delete old transferred records from temp tables
                await DeleteOldTempRecordsAsync(tempRetentionDays);

                // 3. Delete old records from operative tables, keeping unique barcode per branch
                await DeleteOldOperativeRecordsAsync(operRetentionDays);
            }
            catch (Exception ex)
            {
               // Log.Error(ex, "Error occurred in MaintenanceJob.");
                await _databaseLogger.LogErrorAsync("MaintenanceJob", "Error during maintenance tasks", ex);
            }

          //  Log.Information("MaintenanceJob completed.");
        }

        private async Task DeleteOldLogsAsync(int retentionDays)
        {
         //   Log.Information($"Deleting logs older than {retentionDays} days.");

            // Implement the logic to delete old records from logging tables
            // Example for BadItemLog:
            await _badItemLogRepo.DeleteLogsOlderThanAsync(retentionDays);
            await _databaseLogger.DeleteAllLogsOlderThan(retentionDays);
            // Similarly, delete from other logging tables as needed
            // await _databaseLogger.DeleteServiceLogsOlderThanAsync(retentionDays);
            // await _databaseLogger.DeleteErrorLogsOlderThanAsync(retentionDays);
            // etc.

            //  Log.Information("Old logs deleted successfully.");
            await _databaseLogger.LogServiceActionAsync("Old logs deleted successfully.");
        }

        private async Task DeleteOldTempRecordsAsync(int retentionDays)
        {
           // Log.Information($"Deleting temp records older than {retentionDays} days.");

            // Implement the logic to delete old transferred records from temp tables
            await _allItemsRepo.DeleteTransferredItemsOlderThanAsync(retentionDays);
            await _promotionsRepository.DeleteTransferredItemsOlderThanAsync(retentionDays);
            await _priceUpdateRepository.DeleteTransferredItemsOlderThanAsync(retentionDays);
            // Log.Information("Old temp records deleted successfully.");
            await _databaseLogger.LogServiceActionAsync("Old temp records deleted successfully.");
        }

        private async Task DeleteOldOperativeRecordsAsync(int retentionDays)
        {
          //  Log.Information($"Deleting operative records older than {retentionDays} days, keeping unique barcode per branch.");

            // Implement the logic to delete old records from operative tables, ensuring unique barcode per branch
            await _itemsRepo.DeleteOldOperativeRecordsAsync(retentionDays);

          //  Log.Information("Old operative records deleted successfully.");
            await _databaseLogger.LogServiceActionAsync("Old operative records deleted successfully.");
        }
    }
}
