using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Models.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Service.Jobs
{
    public class PriceUpdateJob : IJob
    {
        private readonly IPriceUpdateService _priceUpdateService;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IConfiguration _configuration;
        private readonly IBranchRepository _branchRepository;

        public PriceUpdateJob(
            IPriceUpdateService priceUpdateService,
            IDatabaseLogger databaseLogger,
            IConfiguration configuration,
            IBranchRepository branchRepository)
        {
            _priceUpdateService = priceUpdateService;
            _databaseLogger = databaseLogger;
            _configuration = configuration;
            _branchRepository = branchRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var branches = await _branchRepository.GetAllAsync();
                foreach (var branch in branches)
                {
                    await _priceUpdateService.InsertPriceUpdatesAsync(branch, null);
                }
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("PriceUpdateJob", null, ex);
            }

        }
    }
}
