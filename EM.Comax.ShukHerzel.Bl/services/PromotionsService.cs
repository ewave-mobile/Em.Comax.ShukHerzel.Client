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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.services
{
    public class PromotionsService : IPromotionsService
    {
        private readonly IComaxApiClient _comaxApiClient;
        private readonly IPromotionsRepository _promotionsRepository;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IBranchRepository _branchRepository;
        private readonly OutputSettings _outputSettings;
        public PromotionsService(
     IComaxApiClient comaxApiClient,
     IPromotionsRepository promotionsRepository,
     IDatabaseLogger databaseLogger,
     IBranchRepository branchRepository,
     IOptions<OutputSettings> outputSettings)
        {
            _comaxApiClient = comaxApiClient;
            _promotionsRepository = promotionsRepository;
            _databaseLogger = databaseLogger;
            _branchRepository = branchRepository;
            _outputSettings = outputSettings.Value;
        }

        public Task<List<Promotion>> GetNonTransferredPromotionsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches promotions from Comax, maps them, and inserts into the DB. 
        /// For each 'promotion' from Comax:
        ///   - If it has multiple "Items", we create multiple rows in "Promotion" table 
        ///   - If no items, we just create one row
        /// </summary>
        public async Task InsertPromotionsAsync(
            Branch branch,
            DateTime? lastUpdateDate,
            IProgress<string>? progress = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if(lastUpdateDate == null)
                {
                    if (branch.LastPromotionTimeStamp == null)
                    {
                        lastUpdateDate = DateTime.Now.AddYears(-1);
                    }
                    else
                    {
                        lastUpdateDate = branch.LastPromotionTimeStamp;
                    }
                }

                progress?.Report($"Fetching promotions for branch {branch.Description} since {lastUpdateDate}...");
                await _databaseLogger.LogServiceActionAsync($"Fetching promotions for branch {branch.Description} since {lastUpdateDate}...");

                // 1. Get from Comax
                var promoList = await _comaxApiClient.GetPromotionsAsync(branch, lastUpdateDate ?? DateTime.Now.AddYears(-1), justActive: false, cancellationToken);
                progress?.Report($"Fetched {promoList.d?.Length ?? 0} promotions. Mapping...");
               
                var now = DateTime.Now;
                var operationGuid = Guid.NewGuid();
                var toInsert = new List<Promotion>();
                var promoListJson = JsonConvert.SerializeObject(promoList, Formatting.Indented);
                 await WriteApiResponseToFileAsync(promoListJson, branch, Guid.NewGuid());
                foreach (var clsPromo in promoList?.d ?? [])
                {
                    // If Comax returned multiple "Items" in the promotion, 
                    // we create a separate row for each "Kod" in Items
                    if (clsPromo.Items != null && clsPromo.Items.Any())
                    {
                        // create an string of all the item kods in all the item in clspromo.items

                        var itemKodsArray = clsPromo.Items.Select(x => x.Kod).ToArray();
                        //now turn the array into a string
                        var itemKods = string.Join(",", itemKodsArray);
                        //limit to 200 characters
                        if (itemKods?.Length  > 200)
                        {
                            itemKods = itemKods.Substring(0, 200);
                        }





                        //var itemKods = clsPromo.Items.Select(x => x.Kod).ToArray();

                        foreach (var item in clsPromo.Items)
                        {
                            
                            var entity = MapPromotion(clsPromo, branch, now, operationGuid, itemKod: item.Kod, itemKods );
                            toInsert.Add(entity);
                        }
                    }
                    else
                    {
                        // No child items, just one row
                        var entity = MapPromotion(clsPromo, branch, now, operationGuid, itemKod: null, null);
                        toInsert.Add(entity);
                    }
                }

                progress?.Report($"Mapped {toInsert.Count} promotions. Inserting into DB...");
                // No longer need to set the property on the branch object directly
                // branch.LastPromotionTimeStamp = now;
                await _databaseLogger.LogServiceActionAsync($"Attempting to update LastPromotionTimeStamp for branch {branch.Id} to {now:O} using specific method.");
                await _branchRepository.UpdateLastPromotionTimestampAsync(branch.Id, now); // Call the specific update method
                await _databaseLogger.LogServiceActionAsync($"Successfully called UpdateLastPromotionTimestampAsync for branch {branch.Id}."); // Log completion of the call

                if (toInsert.Any())
                {
                    await _databaseLogger.LogServiceActionAsync($"Attempting to insert {toInsert.Count} promotion entries for branch {branch.Id}.");
                    await _promotionsRepository.BulkInsertAsync(toInsert);
                    await _databaseLogger.LogServiceActionAsync($"Finished inserting {toInsert.Count} promotion entries for branch {branch.Id}.");
                } else {
                    await _databaseLogger.LogServiceActionAsync($"No new promotion entries to insert for branch {branch.Id}.");
                }

                progress?.Report("Promotion processing complete for branch.");
            }
            catch (Exception ex)
            {
                // Log and rethrow
                await _databaseLogger.LogErrorAsync("PROMOTIONS_SERVICE", "InsertPromotionsAsync",ex);
                
            }
        }

        public Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps a single ClsPromotion (+ optional item code) to the EF Promotion entity
        /// </summary>
        private Promotion MapPromotion(D src, Branch branch, DateTime now, Guid guid, string? itemKod, string? anotherBarcodes)
        {
            try
            {
                return new Promotion
                {
                    // Fixed fields/metadata
                    CompanyId = branch.CompanyId ?? Constants.SHUK_HERZEL_COMPANY_ID,
                    BranchId = branch.Id,
                    CreatedDateTime = now,
                    OperationGuid = guid,
                    IsTransferredToOper = false,
                    IsBad = false,

                    // If "itemKod" is not null, use that. Otherwise fall back to src.Kod
                    Kod = src.Kod ?? "",
                    ItemKod = itemKod ?? src.Kod ?? "",
                    Nm = src.Nm ?? "",
                    RemarkForPrint = src.RemarkForPrint ?? "",
                    Nature = src.Nature ?? "",
                    FromDate = src.FromDate ?? "",
                    ToDate = src.ToDate ?? "",
                    SwActive = src.SwActive ?? "",
                    SwSunday = src.SwSunday ?? "",
                    ActiveForHourSunday = src.ActiveFor_Hour_Sunday ?? "",
                    ActiveUpToHourSunday = src.ActiveUpTo_Hour_Sunday ?? "",
                    SwMonday = src.SwMonday ?? "",
                    ActiveForHourMonday = src.ActiveFor_Hour_Monday ?? "",
                    ActiveUpToHourMonday = src.ActiveUpTo_Hour_Monday ?? "",
                    SwTuesday = src.SwTuesday ?? "",
                    ActiveForHourTuesday = src.ActiveFor_Hour_Tuesday ?? "",
                    ActiveUpToHourTuesday = src.ActiveUpTo_Hour_Tuesday ?? "",
                    SwWednesday = src.SwWednesday ?? "",
                    ActiveForHourWednesday = src.ActiveFor_Hour_Wednesday ?? "",
                    ActiveUpToHourWednesday = src.ActiveUpTo_Hour_Wednesday ?? "",
                    SwThursday = src.SwThursday ?? "",
                    ActiveForHourThursday = src.ActiveFor_Hour_Thursday ?? "",
                    ActiveUpToHourThursday = src.ActiveUpTo_Hour_Thursday ?? "",
                    SwFriday = src.SwFriday ?? "",
                    ActiveForHourFriday = src.ActiveFor_Hour_Friday ?? "",
                    ActiveUpToHourFriday = src.ActiveUpTo_Hour_Friday ?? "",
                    SwSaturday = src.SwSaturday ?? "",
                    ActiveForHourSaturday = src.ActiveFor_Hour_Saturday ?? "",
                    ActiveUpToHourSaturday = src.ActiveUpTo_Hour_Saturday ?? "",
                    SwKupa = src.SwKupa ?? "",
                    RealizationPercent = src.RealizationPercent ?? "",
                    SwAllBranches = src.SwAllBranches ?? "",
                    SwAllCustomers = src.SwAllCustomers ?? "",
                    SwAllItems = src.SwAllItems ?? "",
                    SwPrintNm = src.SwPrintNm ?? "",
                    SwSignageOnly = src.SwSignageOnly ?? "",
                    SwCasing = src.SwCasing ?? "",
                    SwIncludeRelatedCompStores = src.SwIncludeRelatedCompStores ?? "",
                    PromotionType = src.PromotionType ?? "",
                    SupplierName = src.SupplierName ?? "",
                    Quantity = src.Quantity ?? "",
                    MinQty = src.MinQty ?? "",
                    MaxQty = src.MaxQty ?? "",
                    Total = src.Total ?? "",
                    GetGiftItem = src.GetGiftItem ?? "",
                    GetRemark = src.GetRemark ?? "",
                    GetCmt = src.GetCmt ?? "",
                    SwIncludeNetoItem = src.SwIncludeNetoItem ?? "",
                    GetTotal = src.GetTotal ?? "",
                    GetDiscountPrecent = src.GetDiscountPrecent ?? "",
                    GetDiscountTotal = src.GetDiscountTotal ?? "",
                    TotalForActivate = src.TotalForActivate ?? "",
                    SwSameDiffItems = src.SwSameDiffItems ?? "",
                    WithoutPrintContent = src.WithoutPrintContent ?? "",
                    Rating = src.Rating ?? "",
                    NoAdditionalDiscounts = src.NoAdditionalDiscounts ?? "",
                    WithoutPresentList = src.WithoutPresentList ?? "",
                    WithoutPrintingData = src.WithoutPrintingData ?? "",
                    Classified = src.Classified ?? "",
                    MaxInDoc = src.MaxInDoc ?? "",
                    SwCalcEnd = src.SwCalcEnd ?? "",
                    SwCheckForTotalNeto = src.SwCheck_ForTotalNeto ?? "",
                    SwCalcDis = src.SwCalcDis ?? "",
                    SwMustPayClubCredit = src.SwMustPay_ClubCredit ?? "",
                    SpurMessage = src.SpurMessage ?? "",
                    SpurTotal = src.SpurTotal ?? "",
                    SpurQty = src.SpurQty ?? "",
                    DoubleDeals = src.DoubleDeals ?? "",
                    WithoutMarkOnWeb = src.WithoutMarkOnWeb ?? "",
                    SwSupplierCharge = src.SwSupplierCharge ?? "",
                    SupplierForCharge = src.SupplierForCharge ?? "",
                    PriceListForCharge = src.PriceListForCharge ?? "",
                    SwChargeType = src.SwChargeType ?? "",
                    TotalDiscountCharge = src.TotalDiscountCharge ?? "",
                    SwOperative = src.SwOperative ?? "",
                    SwNoSplit = src.SwNoSplit ?? "",
                    MustAdditionalPromotions = src.MustAdditionalPromotions ?? "",
                    TextForWeb = src.TextForWeb ?? "",
                    TextToPrint = src.TextToPrint ?? "",
                    TextToPrintUnicode = src.TextToPrint_Unicode ?? "",
                    ApprovedSignage = src.ApprovedSignage ?? "",
                    Tag1 = src.Tag1 ?? "",
                    Tag2 = src.Tag2 ?? "",
                    SelfFinancingReward = src.SelfFinancingReward ?? "",
                    PromoForRealization = src.PromoForRealization ?? "",
                    CostOfRealizingGift = src.CostOfRealizingGift ?? "",
                    SelectPromoToMultiply = src.SelectPromo_ToMultiply ?? "",
                    SelectPromoToNotMultiply = src.SelectPromo_ToNotMultiply ?? "",
                    AnotherBarcodes = anotherBarcodes ?? "",

                    // Convert complex arrays/objects to JSON strings, so they fit into Promotion.* columns
                    Stores = SafeSerializeToJson(src.Stores),
                    CustomerGrp = SafeSerializeToJson(src.CustomerGrp),
                    Items = SafeSerializeToJson(src.Items),
                    Suppliers = SafeSerializeToJson(src.Suppliers),
                    ItemsGrp = SafeSerializeToJson(src.ItemsGrp),
                    ItemsSubGrp = SafeSerializeToJson(src.ItemsSubGrp),
                    ItemsDep = SafeSerializeToJson(src.ItemsDep),
                    ItemsModel = SafeSerializeToJson(src.ItemsModel),
                    ItemsVarious = SafeSerializeToJson(src.ItemsVarious),
                    ItemsAttribute1 = SafeSerializeToJson(src.ItemsAttribute1),
                    ItemsAttribute2 = SafeSerializeToJson(src.ItemsAttribute2),
                    ItemsAttribute3 = SafeSerializeToJson(src.ItemsAttribute3),
                    GetItems = SafeSerializeToJson(src.GetItems),
                    GetSuppliers = SafeSerializeToJson(src.GetSuppliers),
                    GetItemsGrp = SafeSerializeToJson(src.GetItemsGrp),
                    GetItemsSubGrp = SafeSerializeToJson(src.GetItemsSubGrp),
                    GetItemsDep = SafeSerializeToJson(src.GetItemsDep),
                    GetItemsModel = SafeSerializeToJson(src.GetItemsModel),
                    GetItemsAttribute1 = SafeSerializeToJson(src.GetItemsAttribute1),
                    GetItemsAttribute2 = SafeSerializeToJson(src.GetItemsAttribute2),
                    GetItemsAttribute3 = SafeSerializeToJson(src.GetItemsAttribute3),
                    ErrorMessage = SafeSerializeToJson(src.ErrorMessage),

                    ClassifiedNm = src.ClassifiedNm ?? "",
                    CounterGetInSale = src.CounterGet_InSale ?? "",
                    SameGetAndBuy = SafeSerializeToJson(src.SameGetAndBuy)
                };
            }
            catch (Exception ex)
            {

                _databaseLogger.LogErrorAsync("PROMOTIONS_SERVICE", "MapPromotion", ex);
                return new Promotion();

            }
        }
        /// <summary>
        /// Safely serializes an object to JSON, catches exceptions, and truncates to a specified max length.
        /// </summary>
        /// 
        private async Task WriteApiResponseToFileAsync(string apiResponse, Branch branch, Guid guid)
        {
            try
            {
                var directoryPath = _outputSettings.ApiResponseDirectory;

                // Ensure the directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create a unique file name, e.g., including branch ID and timestamp
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"PromotionsResponse_Branch{branch.Id}_{timestamp}_{guid}.txt";
                var filePath = Path.Combine(directoryPath, fileName);

                // Write the API response to the file asynchronously
                await File.WriteAllTextAsync(filePath, apiResponse, Encoding.UTF8);

                await _databaseLogger.LogServiceActionAsync($"API response written to file: {filePath}");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("PROMOTIONS_SERVICE", "WriteApiResponseToFileAsync", ex);
            }
        }
        private string SafeSerializeToJson(object data, int maxLength = 2000)
        {
            if (data == null) return string.Empty;

            try
            {
                var json = JsonConvert.SerializeObject(data);
                if (json.Length > maxLength)
                {
                    json = json.Substring(0, maxLength);
                }
                return json;
            }
            catch
            {
                // Optionally, you could log the error here or set a flag.
                return string.Empty;
            }
        }

    }
}
