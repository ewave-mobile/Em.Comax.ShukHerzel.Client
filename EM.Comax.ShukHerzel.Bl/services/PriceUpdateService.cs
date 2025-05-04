using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.services
{
    public class PriceUpdateService : IPriceUpdateService
    {
        private readonly IComaxApiClient _comaxApiClient;
        private readonly IPriceUpdateRepository _priceUpdateRepository;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IBranchRepository _branchRepository;
        private readonly OutputSettings _outputSettings;

        public PriceUpdateService(
            IComaxApiClient comaxApiClient,
            IPriceUpdateRepository priceUpdateRepository,
            IDatabaseLogger databaseLogger,
            IBranchRepository branchRepository,
            IOptions<OutputSettings> outputSettings)
        {
            _comaxApiClient = comaxApiClient;
            _priceUpdateRepository = priceUpdateRepository;
            _databaseLogger = databaseLogger;
            _branchRepository = branchRepository;
            _outputSettings = outputSettings.Value;
        }

        /// <summary>
        /// Fetches new price updates from Comax, maps them to PriceUpdate entities, and inserts them into the database.
        /// </summary>
        public async Task InsertPriceUpdatesAsync(
            Branch branch,
            DateTime? lastUpdateDate,
            IProgress<string>? progress = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Determine the starting date: if no lastUpdateDate is provided, use the branch’s last update (or default one year back).
                if (lastUpdateDate == null)
                {
                    if (branch.LastPriceTimeStamp == null)
                    {
                        lastUpdateDate = DateTime.Now.AddYears(-1);
                    }
                    else
                    {
                        lastUpdateDate = branch.LastPriceTimeStamp;
                    }
                }

                progress?.Report($"Fetching price updates for branch {branch.Description} since {lastUpdateDate}...");
                await _databaseLogger.LogServiceActionAsync($"Fetching price updates for branch {branch.Description} since {lastUpdateDate}...");

                // 1. Get new price data from Comax.
                var priceList = await _comaxApiClient.GetNewPricesAsync(branch, lastUpdateDate.Value, cancellationToken);

                progress?.Report($"Fetched {priceList.Count} price updates. Mapping...");

                var now = DateTime.Now;
                var operationGuid = Guid.NewGuid();
                var toInsert = new List<PriceUpdate>();

                // Optionally log the raw API response
                var priceListJson = JsonConvert.SerializeObject(priceList, Formatting.Indented);
                await WriteApiResponseToFileAsync(priceListJson, branch, Guid.NewGuid());

                // 2. Map each returned DTO to the PriceUpdate model.
                foreach (var item in priceList)
                {
                    var entity = MapPriceUpdate(item, branch, now, operationGuid);
                    toInsert.Add(entity);
                }

                progress?.Report($"Mapped {toInsert.Count} price updates. Inserting into DB...");

                // Update the branch’s last price update timestamp using the specific method
                // branch.LastPriceTimeStamp = now; // No longer needed
                await _databaseLogger.LogServiceActionAsync($"Attempting to update LastPriceTimeStamp for branch {branch.Id} to {now:O} using specific method.");
                await _branchRepository.UpdateLastPriceTimestampAsync(branch.Id, now); // Call the specific update method
                await _databaseLogger.LogServiceActionAsync($"Successfully called UpdateLastPriceTimestampAsync for branch {branch.Id}.");

                // 3. Insert the new records into the database (using a bulk insert for performance).
                 if (toInsert.Any())
                {
                    await _databaseLogger.LogServiceActionAsync($"Attempting to insert {toInsert.Count} price update entries for branch {branch.Id}.");
                    await _priceUpdateRepository.BulkInsertAsync(toInsert);
                    await _databaseLogger.LogServiceActionAsync($"Finished inserting {toInsert.Count} price update entries for branch {branch.Id}.");
                } else {
                     await _databaseLogger.LogServiceActionAsync($"No new price update entries to insert for branch {branch.Id}.");
                }

                progress?.Report("Price updates inserted successfully.");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("PRICE_UPDATE_SERVICE", "InsertPriceUpdatesAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves price updates that have not yet been transferred.
        /// </summary>
        public async Task<List<PriceUpdate>> GetNonTransferredPriceUpdatesAsync()
        {
            // The repository should implement logic to return only records where IsTransferredToOper == false.
            return await _priceUpdateRepository.GetNonTransferredItemsAsync();
        }

        /// <summary>
        /// Marks the given price update IDs as transferred.
        /// </summary>
        public async Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime)
        {
            await _priceUpdateRepository.MarkAsTransferredAsync(ids, transferredTime);
        }

        /// <summary>
        /// Maps an individual ItemSalePriceDto (from Comax) to a PriceUpdate entity.
        /// </summary>
        private PriceUpdate MapPriceUpdate(ItemSalePriceDto item, Branch branch, DateTime now, Guid operationGuid)
        {
            // The nested SalesPrice property holds numeric and currency information.
            var salesPrice = item.SalesPrice.ClsItemPrices;

            return new PriceUpdate
            {
                CompanyId = branch.CompanyId ?? Constants.SHUK_HERZEL_COMPANY_ID,
                BranchId = branch.Id,
                CreatedDateTime = now,
                IsTransferredToOper = false,
                TransferredDateTime = null,
                OperationGuid = operationGuid,
                Name = item.Name ?? string.Empty,
                // Map additional properties as needed. For example:
                Size = string.Empty, // Populate if your DTO has size information.
                XmlId = item.ID ?? string.Empty,
                Barcode = item.Barcode ?? string.Empty,
                AlternateId = item.AlternateID ?? string.Empty,
                PriceListId = salesPrice?.PriceListID.ToString() ?? string.Empty,
                Currency = salesPrice?.Currency ?? string.Empty,
                IsIncludeVat = salesPrice != null ? salesPrice.IsIncludeVat.ToString() : string.Empty,
                Price = salesPrice?.Price.ToString() ?? string.Empty,
                NetPrice = salesPrice?.NetPrice.ToString() ?? string.Empty,
                ShekelPrice = salesPrice?.ShekelPrice.ToString() ?? string.Empty,
                ShekelNetPrice = salesPrice?.ShekelNetPrice.ToString() ?? string.Empty,
                SalePrice = salesPrice?.SalePrice.ToString() ?? string.Empty,
                OperationEndDate = salesPrice?.OperationEndDate ?? string.Empty
            };
        }

        /// <summary>
        /// Writes the raw API response to a file (for troubleshooting or logging purposes).
        /// </summary>
        private async Task WriteApiResponseToFileAsync(string apiResponse, Branch branch, Guid guid)
        {
            try
            {
                var directoryPath = _outputSettings.ApiResponseDirectory;
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"PriceUpdatesResponse_Branch{branch.Id}_{timestamp}_{guid}.txt";
                var filePath = Path.Combine(directoryPath, fileName);
                await File.WriteAllTextAsync(filePath, apiResponse, Encoding.UTF8);
                await _databaseLogger.LogServiceActionAsync($"API response written to file: {filePath}");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("PRICE_UPDATE_SERVICE", "WriteApiResponseToFileAsync", ex);
            }
        }
    }
}
