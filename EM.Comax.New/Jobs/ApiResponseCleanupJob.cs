using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzl.Common;
using Microsoft.Extensions.Options;
using Quartz;

namespace EM.Comax.New.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class ApiResponseCleanupJob : IJob
    {
        private readonly IDatabaseLogger _databaseLogger;
        private readonly OutputSettings _outputSettings;

        public ApiResponseCleanupJob(
            IDatabaseLogger databaseLogger,
            IOptions<OutputSettings> outputSettings)
        {
            _databaseLogger = databaseLogger;
            _outputSettings = outputSettings.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var result = ApiResponseCleanupHelper.Cleanup(
                    _outputSettings.ApiResponseDirectory,
                    _outputSettings.ApiResponseRetentionDays);

                foreach (var (filePath, fileEx) in result.FileErrors)
                {
                    await _databaseLogger.LogErrorAsync(
                        "ApiResponseCleanupJob",
                        $"Failed to delete file: {filePath}",
                        fileEx);
                }

                await _databaseLogger.LogServiceActionAsync(result.Message);
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync(
                    "ApiResponseCleanupJob",
                    "Error during API response file cleanup",
                    ex);
            }
        }
    }
}
