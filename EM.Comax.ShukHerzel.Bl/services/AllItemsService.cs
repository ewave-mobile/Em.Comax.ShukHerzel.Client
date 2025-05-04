using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EM.Comax.ShukHerzel.Bl.services
{
    public class AllItemsService : IAllItemsService
    {
        private readonly IComaxApiClient _comaxApiClient;
        private readonly IAllItemsRepository _allItemsRepository;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IBranchRepository _branchRepository;
        private readonly OutputSettings _outputSettings;

  

public AllItemsService(
    IComaxApiClient comaxApiClient,
    IAllItemsRepository allItemsRepository,
    IDatabaseLogger databaseLogger,
    IBranchRepository branchRepository,
    IOptions<OutputSettings> outputSettings)
{
    _comaxApiClient = comaxApiClient;
    _allItemsRepository = allItemsRepository;
    _databaseLogger = databaseLogger;
    _branchRepository = branchRepository;
    _outputSettings = outputSettings.Value;
}


        public List<ArrayOfClsItemsClsItems> DeserializeCatalogItems(string xml)
        {
            var serializer = new XmlSerializer(typeof(ArrayOfClsItems),
        new XmlRootAttribute("ArrayOfClsItems")
        {
            Namespace = "http://ws.comax.co.il/Comax_WebServices/"
        });

            using var reader = new StringReader(xml);
            var result = (ArrayOfClsItems)serializer.Deserialize(reader);
            return result.ClsItems.ToList();
        }

        public Task<List<AllItem>> GetNonTransferredItemsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task InsertAllItemsAsync(List<AllItem> allItems)
        {
           await _allItemsRepository.BulkInsertAsync(allItems);
        }

        public async Task InsertCatalogAsync( Branch branch, 
            DateTime? lastUpdateDate,
            IProgress<string>? progress = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (lastUpdateDate == null)
                {
                    if(branch.LastCatalogTimeStamp == null)
                    {
                        lastUpdateDate = DateTime.Now.AddYears(-1);
                    }
                    else
                    {
                        lastUpdateDate = branch.LastCatalogTimeStamp;
                    }
                }
                var guid = Guid.NewGuid();
                var now = DateTime.Now;
                await _databaseLogger.LogServiceActionAsync($"Getting catalog items from Comax API for branch {branch.Description}...");
                progress?.Report($"Getting catalog items from Comax API for branch {branch.Description}, since {lastUpdateDate ?? DateTime.Now.AddYears(-1)}...");
                var items = await _comaxApiClient.GetCatalogXmlAsync(branch, lastUpdateDate ?? DateTime.Now.AddYears(-1), cancellationToken);
                progress?.Report("Deserializing catalog items...");
                await WriteApiResponseToFileAsync(items, branch, guid);
                var clsItems = DeserializeCatalogItems(items);
                progress?.Report($"recieved {clsItems.Count} items, Mapping catalog items...");
                var allItems = await MapToAllItems(clsItems, now, branch, guid);
                progress?.Report("Inserting catalog items to DB...");
                if (allItems.Count > 0)
                {
                    await InsertAllItemsAsync(allItems);
                    await _databaseLogger.LogServiceActionAsync($"Finished inserting {allItems.Count} catalog items for branch {branch.Id}.");
                } else {
                     await _databaseLogger.LogServiceActionAsync($"No new catalog items to insert for branch {branch.Id}.");
                }

                // No longer need to set the property on the branch object directly
                // branch.LastCatalogTimeStamp = now;
                await _databaseLogger.LogServiceActionAsync($"Attempting to update LastCatalogTimeStamp for branch {branch.Id} to {now:O} using specific method.");
                await _branchRepository.UpdateLastCatalogTimestampAsync(branch.Id, now); // Call the specific update method
                await _databaseLogger.LogServiceActionAsync($"Successfully called UpdateLastCatalogTimestampAsync for branch {branch.Id}."); // Log completion of the call
                progress?.Report("Catalog processing complete for branch.");

            }
            catch (Exception ex) {
                await _databaseLogger.LogErrorAsync("ALL_ITEMS_SERVICE", "InsertCatalogAsync", ex);
            }
        }

        public async Task<List<AllItem>> MapToAllItems(
      List<ArrayOfClsItemsClsItems> clsItems,
      DateTime createDate,
      Branch branch,
      Guid operationGuid)
        {
            try
            {
                // Guard checks if desired:
                // if (branch == null) throw new ArgumentNullException(nameof(branch));
                // if (clsItems == null) throw new ArgumentNullException(nameof(clsItems));

                var allItemsList = clsItems.Select(c => new AllItem
                {
                    // Common fields
                    CompanyId = branch.CompanyId ?? -1,
                    BranchId = branch.Id,
                    CreatedDateTime = createDate,
                    IsTransferredToOper = false,
                    IsBad = false,
                    OperationGuid = operationGuid,

                    // Map from Comax object to AllItem columns
                    Name = c.Name,
                    Size = c.Size,
                    XmlId = c.ID.ToString(),
                    OldId = c.OldID.ToString(),
                    AnotherBarkods = c.AnotherBarkods?.ToString(),
                    WebShortName = c.WebShortName?.ToString(),
                    WebDesc = c.WebDesc?.ToString(),
                    WebDesc2 = c.WebDesc2?.ToString(),
                    WebDelvDesc = c.WebDelvDesc?.ToString(),
                    WebInstruction = c.WebInstruction?.ToString(),
                    Barcode = c.Barcode.ToString(),
                    PurchasingAccountId = c.PurchasingAccountID.ToString(),
                    SalesAccountId = c.SalesAccountID.ToString(),
                    AlternateId = c.AlternateID?.ToString(),
                    WorkByAlternateId = c.WorkByAlternateID.ToString(),
                    SerialNo = c.SerialNo?.ToString(),

                    Price = c.Price.ToString(),
                    Quantity = c.Quantity.ToString(),
                    DiscountPercent = c.DiscountPercent.ToString(),
                    DiscountTotal = c.DiscountTotal.ToString(),
                    TotalSum = c.TotalSum.ToString(),
                    MaxQuantityInOrder = c.MaxQuantityInOrder.ToString(),
                    CorrectionPrice = c.CorrectionPrice.ToString(),
                    CorrectionDiscountPercent = c.CorrectionDiscountPercent.ToString(),
                    CorrectionQuantity = c.CorrectionQuantity.ToString(),
                    ConversionQuantity = c.ConversionQuantity.ToString(),

                    SuperDepartment = c.SuperDepartment,
                    SuperDepartmentCode = c.SuperDepartmentCode.ToString(),
                    Department = c.Department,
                    DepartmentCode = c.DepartmentCode.ToString(),
                    DepartmentEnglishName = c.DepartmentEnglishName?.ToString(),

                    GroupName = c.Group,
                    GroupCode = c.GroupCode.ToString(),
                    GroupEnglishName = c.GroupEnglishName?.ToString(),
                    SubGroup = c.Sub_Group,
                    SubGroupCode = c.Sub_GroupCode.ToString(),
                    SubGroupEnglishName = c.Sub_GroupEnglishName?.ToString(),

                    Manufacturer = c.Manufacturer,
                    ManufacturerCode = c.ManufacturerCode.ToString(),

                    // Nested objects: Model/Color/Size
                    ItemModelName = c.ItemModel?.Name?.ToString(),
                    ItemModelId = c.ItemModel?.ID?.ToString(),
                    ItemColorName = c.ItemColor?.Name?.ToString(),
                    ItemColorId = c.ItemColor?.ID?.ToString(),
                    ItemSizeName = c.ItemSize?.Name?.ToString(),
                    ItemSizeId = c.ItemSize?.ID?.ToString(),
                    ItemsTree = c.ItemsTree?.ToString(),

                    Attribute1 = c.Attribute1?.ToString(),
                    Attribute1Code = c.Attribute1Code?.ToString(),
                    Attribute2 = c.Attribute2?.ToString(),
                    Attribute2Code = c.Attribute2Code?.ToString(),
                    Attribute3 = c.Attribute3?.ToString(),
                    Attribute3Code = c.Attribute3Code?.ToString(),
                    Attribute4 = c.Attribute4?.ToString(),
                    Attribute4Code = c.Attribute4Code?.ToString(),
                    Attribute5 = c.Attribute5?.ToString(),
                    Attribute5Code = c.Attribute5Code?.ToString(),

                    SupplierId = c.SupplierID.ToString(),
                    SupplierName = c.SupplierName,
                    SupplierCatalogNumber = c.SupplierCatalogNumber?.ToString(),
                    SupplierPrice = c.SupplierPrice.ToString(),
                    SupplierCurrency = c.SupplierCurrency?.ToString(),
                    SupplierShekelPrice = c.SupplierShekelPrice.ToString(),
                    SupplierPriceNet = c.SupplierPriceNet.ToString(),
                    SupplierShekelPriceNet = c.SupplierShekelPriceNet.ToString(),
                        
                    ArchiveDate = c.ArchiveDate?.ToString(),
                    BlockPurchaseDate = c.BlockPurchaseDate?.ToString(),
                    BlockSalesDate = c.BlockSalesDate?.ToString(),
                    EnglishName = c.EnglishName?.ToString(),
                    PromoId = c.PromoID.ToString(),
                    PromoRank = c.PromoRank.ToString(),
                    MatchingItemId = c.MatchingItemID.ToString(),
                    MatchingItemQuantity = c.MatchingItemQuantity.ToString(),
                    UnitsQuantity = c.UnitsQuantity.ToString(),
                    Length = c.Length.ToString(),
                    LengthSize = c.LengthSize?.ToString(),
                    Width = c.Width.ToString(),
                    WidthSize = c.WidthSize?.ToString(),
                    Height = c.Height.ToString(),
                    HeightSize = c.HeightSize?.ToString(),
                    Weight = c.Weight.ToString(),
                    WeightSize = c.WeightSize?.ToString(),
                    CurrentStoreId = c.CurrentStoreID.ToString(),
                    BaseLine = c.BaseLine.ToString(),
                    BaseDocNumber = c.BaseDocNumber.ToString(),
                    BaseReference = c.BaseReference.ToString(),
                    BaseYear = c.BaseYear.ToString(),
                    Conversion = c.Conversion.ToString(),
                    AdditionalConversion = c.AdditionalConversion.ToString(),
                    NotShowInWeb = c.NotShowInWeb.ToString(),
                    LinkWebSite = c.LinkWebSite?.ToString(),
                    SetModelProperties = c.SetModelProperties.ToString(),
                    HierarchyUpdate = c.HierarchyUpdate.ToString(),
                    DownloadPriceTransparencyAfter48Hours = c.DownloadPriceTransparencyAfter48Hours,
                    ItemData = c.ItemData,
                    RejectTypeId = c.RejectTypeID.ToString(),
                    OpenDate = c.OpenDate,
                    WarrantyMonths = c.WarrantyMonths.ToString(),
                    Ingredients = c.Ingredients?.ToString(),
                    FullIngredientsReplace = c.FullIngredientsReplace.ToString(),
                    CmtAmara = c.CmtAmara.ToString(),
                    AmaraBy = c.AmaraBy,
                    CmtAmara2 = c.CmtAmara2.ToString(),
                    InternalAmara = c.InternalAmara.ToString(),
                    Line = c.Line.ToString(),
                    LengthOptic = c.LengthOptic.ToString(),
                    DiagonalOptic = c.DiagonalOptic.ToString(),
                    HeightOptic = c.HeightOptic.ToString(),
                    NameInWeb = c.NameInWeb?.ToString(),
                    //NosafKod2 = c.NosafKod_2?.ToString(),
                    //NosafNm2 = c.NosafNm_2?.ToString(),
                    //NosafKod3 = c.NosafKod_3?.ToString(),
                    //NosafNm3 = c.NosafNm_3?.ToString(),
                    //AtarDescription = c.AtarDescription?.ToString(),
                    //Energia = c.Energia?.ToString(),
                    //Pahmemot = c.Pahmemot?.ToString(),
                    //Helbonim = c.Helbonim?.ToString(),
                    //Shumanim = c.Shumanim?.ToString(),
                    //Colestrol = c.Colestrol?.ToString(),
                    //MhrMivza = c.MhrMivza?.ToString(),
                    //SwBonos = c.SwBonos?.ToString(),
                    //BonosFline = c.BonosFline?.ToString(),
                    //BonosTline = c.BonosTline?.ToString(),
                    //MirshamPhrmaSoft = c.Mirsham_PhrmaSoft?.ToString(),
                    //CustomerMakat = c.CustomerMakat?.ToString(),
                    //QtyType = c.QtyType,
                    //NoDiscount = c.NoDiscount?.ToString(),
                    //PriceUsd = c.PriceUSD?.ToString(),
                    //PriceNis = c.PriceNIS?.ToString(),
                    //WarrantySupplier = c.WarrantySupplier?.ToString(),
                    //WarrantyRemark = c.WarrantyRemark?.ToString(),
                    Content = c.Content?.ToString(),
                    ContentUnit = c.ContentUnit,
                    ContentMeasure = c.ContentMeasure?.ToString(),
                    //Volume = c.Volume?.ToString(),
                    //VolumeSize = c.VolumeSize?.ToString(),
                    //SwWeighable = c.SwWeighable?.ToString(),
                    //Radius = c.Radius?.ToString(),
                    //Nava = c.Nava?.ToString(),
                    //Thickness = c.Thickness?.ToString(),
                    //MinStock = c.MinStock?.ToString(),
                    //MaxStock = c.MaxStock?.ToString(),
                    //SuppliersOrders = c.SuppliersOrders?.ToString(),
                    //CustomersOrders = c.CustomersOrders?.ToString(),
                    //SwSerialy = c.SwSerialy?.ToString(),
                    //StoreId = c.StoreID?.ToString(),
                    //NetPrice = c.NetPrice?.ToString(),
                    //CmtAmr3 = c.CmtAmr3?.ToString(),
                    //MidaAmr3 = c.MidaAmr3?.ToString(),
                    //NmMashlim = c.Nm_Mashlim?.ToString(),
                    //WebManufacturer = c.WebManufacturer?.ToString(),
                    //WebManufacturerCode = c.WebManufacturerCode?.ToString(),
                    //WebCategory = c.WebCategory?.ToString(),
                    //WebSubGroup = c.WebSub_Group?.ToString(),
                    //WebColor = c.WebColor?.ToString(),
                    //WebDep = c.WebDep?.ToString(),
                    //WebGrp = c.WebGrp?.ToString(),
                    //WebNosafKod2 = c.WebNosafKod_2?.ToString(),
                    //WebNosafNm2 = c.WebNosafNm_2?.ToString(),
                    //IsGiftCardPurchase = c.IsGiftCardPurchase?.ToString(),
                    //WebColorNm = c.WebColorNm?.ToString(),
                    //WebDepNm = c.WebDepNm?.ToString(),
                    //WebGrpNm = c.WebGrpNm?.ToString(),
                    //WebSubGroupNm = c.WebSub_GroupNm?.ToString(),
                    //WebCategoryNm = c.WebCategoryNm?.ToString(),
                    //WebRating = c.WebRating?.ToString(),
                    //LimitAge = c.LimitAge?.ToString(),
                    SwPikadon = c.SwPikadon?.ToString(),
                    TrailingItem = c.TrailingItem?.ToString(),
                    //KodSet = c.KodSet?.ToString(),
                    //SetId = c.SetId?.ToString(),
                    //MinimumStock = c.MinimumStock?.ToString(),
                    //MaximumStock = c.MaximumStock?.ToString(),
                    //StiaAm = c.StiaAM?.ToString(),
                    //NosafKod4 = c.NosafKod_4?.ToString(),
                    //NosafNm4 = c.NosafNm_4?.ToString(),
                    //NosafKod5 = c.NosafKod_5?.ToString(),
                    //NosafNm5 = c.NosafNm_5?.ToString(),
                    //NosafKod6 = c.NosafKod_6?.ToString(),
                    //NosafNm6 = c.NosafNm_6?.ToString(),
                    //NosafKod7 = c.NosafKod_7?.ToString(),
                    //NosafNm7 = c.NosafNm_7?.ToString(),
                    //NosafKod8 = c.NosafKod_8?.ToString(),
                    //NosafNm8 = c.NosafNm_8?.ToString(),
                    //NosafKod9 = c.NosafKod_9?.ToString(),
                    //NosafNm9 = c.NosafNm_9?.ToString(),
                    //NosafKod10 = c.NosafKod_10?.ToString(),
                    //NosafNm10 = c.NosafNm_10?.ToString(),
                    //SwAlcohol = c.SwAlcohol?.ToString(),
                    //SwMustCmtInKupa = c.SwMustCmtInKupa?.ToString(),
                    ManufacturingCountry = c.ManufacturingCountry?.ToString(),
                    //PikadonCmt = c.PikadonCmt?.ToString(),
                    //IsControlledItem = c.IsControlledItem?.ToString(),
                    //MinQtyForSale = c.MinQtyForSale?.ToString(),
                    //MaxQtyForSale = c.MaxQtyForSale?.ToString(),
                    //PrtC = c.PrtC?.ToString(),
                    //ColorGrp = c.ColorGrp?.ToString(),
                    //RefLine = c.Ref_Line?.ToString()
                }).ToList();

                return allItemsList;
            }
            catch (Exception ex)
            {
                // throw ex;
                // Log the error and return an empty list
               // await _databaseLogger.LogErrorAsync("ALL_ITEMS_SERVICE", "MapToAllItems", ex);
                return new List<AllItem>();
            }
            }
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
                var fileName = $"CatalogResponse_Branch{branch.Id}_{timestamp}_{guid}.xml";
                var filePath = Path.Combine(directoryPath, fileName);

                // Write the API response to the file asynchronously
                await File.WriteAllTextAsync(filePath, apiResponse, Encoding.UTF8);

                await _databaseLogger.LogServiceActionAsync($"API response written to file: {filePath}");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("ALL_ITEMS_SERVICE", "WriteApiResponseToFileAsync", ex);
            }
        }

        public Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime)
        {
            throw new NotImplementedException();
        }
    }
}
