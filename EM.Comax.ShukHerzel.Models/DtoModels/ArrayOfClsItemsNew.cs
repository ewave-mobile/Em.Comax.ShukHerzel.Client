using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    /// <remarks/>  
    [Serializable]
    [XmlRoot("ArrayOfClsItemsWithoutSupplierdata", Namespace = "")]
    public class ArrayOfClsItemsWithoutSupplierdata
    {
        [XmlElement("ClsItemsWithoutSupplierdata")]
        public List<ClsItemsWithoutSupplierdata>? ClsItems { get; set; }
    }

    /// <remarks/>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class ClsItemsWithoutSupplierdata
    {
        public string Name { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public long ID { get; set; } = 0;
        public long OldID { get; set; } = 0;
        public string AnotherBarkods { get; set; } = string.Empty;
        public string WebShortName { get; set; } = string.Empty;
        public string WebDesc { get; set; } = string.Empty;
        public string WebDesc2 { get; set; } = string.Empty;
        public string WebDelvDesc { get; set; } = string.Empty;
        public string WebInstruction { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public long PurchasingAccountID { get; set; } = 0;
        public long SalesAccountID { get; set; } = 0;
        public string AlternateID { get; set; } = string.Empty;
        public bool WorkByAlternateID { get; set; } = false;
        public string SerialNo { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;
        public decimal DiscountPercent { get; set; } = 0;
        public decimal DiscountTotal { get; set; } = 0;
        public decimal TotalSum { get; set; } = 0;
        public decimal MaxQuantityInOrder { get; set; } = 0;
        public decimal CorrectionPrice { get; set; } = 0;
        public decimal CorrectionDiscountPercent { get; set; } = 0;
        public decimal CorrectionQuantity { get; set; } = 0;
        public decimal ConversionQuantity { get; set; } = 0;

        public string SuperDepartment { get; set; } = string.Empty;
        public long SuperDepartmentCode { get; set; } = 0;

        public string Department { get; set; } = string.Empty;
        public long DepartmentCode { get; set; } = 0;
        public string DepartmentEnglishName { get; set; } = string.Empty;

        public string Group { get; set; } = string.Empty;
        public long GroupCode { get; set; } = 0;
        public string? GroupEnglishName { get; set; }
        public string Sub_Group { get; set; } = string.Empty;
        public long Sub_GroupCode { get; set; } = 0;
        public string? Sub_GroupEnglishName { get; set; }

        public string Manufacturer { get; set; } = string.Empty;
        public long ManufacturerCode { get; set; } = 0;

        public ItemModelNew? ItemModel { get; set; } = new();
        public ItemColorNew? ItemColor { get; set; } = new();
        public ItemSizeNew? ItemSize { get; set; } = new();

        public string Attribute1 { get; set; } = string.Empty;
        public string Attribute1Code { get; set; } = string.Empty;
        public string? Attribute2 { get; set; }
        public string? Attribute2Code { get; set; }
        public string? Attribute3 { get; set; }
        public string? Attribute3Code { get; set; }
        public string? Attribute4 { get; set; }
        public string? Attribute4Code { get; set; }
        public string? Attribute5 { get; set; }
        public string? Attribute5Code { get; set; }

        public long SupplierID { get; set; } = 0;
        public string SupplierName { get; set; } = string.Empty;
        public string? ArchiveDate { get; set; }
        public string? BlockPurchaseDate { get; set; }
        public string? BlockSalesDate { get; set; }
        public string? EnglishName { get; set; }

        public long PromoID { get; set; } = 0;
        public long PromoRank { get; set; } = 0;
        public long MatchingItemID { get; set; } = 0;
        public long MatchingItemQuantity { get; set; } = 0;
        public long UnitsQuantity { get; set; } = 0;

        public decimal Length { get; set; } = 0;
        public string? LengthSize { get; set; }
        public decimal Width { get; set; } = 0;
        public string? WidthSize { get; set; } 
        public decimal Height { get; set; } = 0;
        public string? HeightSize { get; set; }
        public decimal Weight { get; set; } = 0;
        public string? WeightSize { get; set; } 

        public string PicURL { get; set; } = string.Empty;
        public long CurrentStoreID { get; set; } = 0;
        public long BaseLine { get; set; } = 0;
        public long BaseDocNumber { get; set; } = 0;
        public long BaseReference { get; set; } = 0;
        public long BaseYear { get; set; } = 0;
        public decimal Conversion { get; set; } = 0;
        public decimal AdditionalConversion { get; set; } = 0;

        public bool NotShowInWeb { get; set; } = false;
        public string LinkWebSite { get; set; } = string.Empty;
        public bool SetModelProperties { get; set; } = false;
        public bool HierarchyUpdate { get; set; } = false;
        public string DownloadPriceTransparencyAfter48Hours { get; set; } = string.Empty;

        public string ItemData { get; set; } = string.Empty;
        public long RejectTypeID { get; set; } = 0;
        public string OpenDate { get; set; } = string.Empty;
        public long WarrantyMonths { get; set; } = 0;
        public string Ingredients { get; set; } = string.Empty;
        public bool FullIngredientsReplace { get; set; } = false;

        public decimal CmtAmara { get; set; } = 0;
        public string AmaraBy { get; set; } = string.Empty;
        public decimal CmtAmara2 { get; set; } = 0;
        public decimal InternalAmara { get; set; } = 0;
        public long Line { get; set; } = 0;
        public decimal LengthOptic { get; set; } = 0;
        public decimal DiagonalOptic { get; set; } = 0;
        public decimal HeightOptic { get; set; } = 0;
        public string NameInWeb { get; set; } = string.Empty;

        public string NosafKod_2 { get; set; } = string.Empty;
        public string NosafNm_2 { get; set; } = string.Empty;
        public string NosafKod_3 { get; set; } = string.Empty;
        public string NosafNm_3 { get; set; } = string.Empty;

        public string AtarDescription { get; set; } = string.Empty;

        public decimal Energia { get; set; } = 0;
        public decimal Pahmemot { get; set; } = 0;
        public decimal Helbonim { get; set; } = 0;
        public decimal Shumanim { get; set; } = 0;
        public decimal Colestrol { get; set; } = 0;
        public decimal MhrMivza { get; set; } = 0;

        public bool SwBonos { get; set; } = false;
        public long BonosFline { get; set; } = 0;
        public long BonosTline { get; set; } = 0;

        public long Mirsham_PhrmaSoft { get; set; } = 0;
        public long CustomerMakat { get; set; } = 0;
        public string QtyType { get; set; } = string.Empty;
        public long NoDiscount { get; set; } = 0;

        public decimal PriceUSD { get; set; } = 0;
        public decimal PriceNIS { get; set; } = 0;

        public long WarrantySupplier { get; set; } = 0;
        public string WarrantyRemark { get; set; } = string.Empty;

        public decimal? Content { get; set; }
        public string? ContentUnit { get; set; }
        public decimal? ContentMeasure { get; set; }

        public decimal Volume { get; set; } = 0;
        public string VolumeSize { get; set; } = string.Empty;

        public bool SwWeighable { get; set; } = false;
        public decimal Radius { get; set; } = 0;
        public decimal Nava { get; set; } = 0;
        public decimal Thickness { get; set; } = 0;

        public decimal MinStock { get; set; } = 0;
        public decimal MaxStock { get; set; } = 0;
        public decimal SuppliersOrders { get; set; } = 0;
        public decimal CustomersOrders { get; set; } = 0;

        public bool SwSerialy { get; set; } = false;
        public long StoreID { get; set; } = 0;

        public decimal NetPrice { get; set; } = 0;

        public decimal CmtAmr3 { get; set; } = 0;
        public string MidaAmr3 { get; set; } = string.Empty;
        public string Nm_Mashlim { get; set; } = string.Empty;

        public string WebManufacturer { get; set; } = string.Empty;
        public long WebManufacturerCode { get; set; } = 0;

        public string WebCategory { get; set; } = string.Empty;
        public long WebSub_Group { get; set; } = 0;
        public long WebColor { get; set; } = 0;
        public long WebDep { get; set; } = 0;
        public long WebGrp { get; set; } = 0;

        public string WebNosafKod_2 { get; set; } = string.Empty;
        public string WebNosafNm_2 { get; set; } = string.Empty;

        public bool IsGiftCardPurchase { get; set; } = false;

        public string WebColorNm { get; set; } = string.Empty;
        public string WebDepNm { get; set; } = string.Empty;
        public string WebGrpNm { get; set; } = string.Empty;
        public string WebSub_GroupNm { get; set; } = string.Empty;
        public string WebCategoryNm { get; set; } = string.Empty;

        public decimal WebRating { get; set; } = 0;
        public long LimitAge { get; set; } = 0;

        public string? SwPikadon { get; set; }
        public string? TrailingItem { get; set; }

        public long KodSet { get; set; } = 0;
        public long SetId { get; set; } = 0;

        public decimal MinimumStock { get; set; } = 0;
        public decimal MaximumStock { get; set; } = 0;

        public decimal StiaAM { get; set; } = 0;

        public string NosafKod_4 { get; set; } = string.Empty;
        public string NosafNm_4 { get; set; } = string.Empty;
        public string NosafKod_5 { get; set; } = string.Empty;
        public string NosafNm_5 { get; set; } = string.Empty;
        public string NosafKod_6 { get; set; } = string.Empty;
        public string NosafNm_6 { get; set; } = string.Empty;
        public string NosafKod_7 { get; set; } = string.Empty;
        public string NosafNm_7 { get; set; } = string.Empty;
        public string NosafKod_8 { get; set; } = string.Empty;
        public string NosafNm_8 { get; set; } = string.Empty;
        public string NosafKod_9 { get; set; } = string.Empty;
        public string NosafNm_9 { get; set; } = string.Empty;
        public string NosafKod_10 { get; set; } = string.Empty;
        public string NosafNm_10 { get; set; } = string.Empty;

        public decimal SwAlcohol { get; set; } = 0;
        public decimal SwMustCmtInKupa { get; set; } = 0;

        public string ManufacturingCountry { get; set; } = string.Empty;

        public decimal PikadonCmt { get; set; } = 0;
        public decimal IsControlledItem { get; set; } = 0;

        public decimal MinQtyForSale { get; set; } = 0;
        public decimal MaxQtyForSale { get; set; } = 0;

        public string SalesList { get; set; } = string.Empty;
    }


    public class ItemModelNew
    {
        public string Name { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty;
    }

    public class ItemColorNew
    {
        public string Name { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty;
    }

    public class ItemSizeNew
    {
        public string Name { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty;
    }
}
