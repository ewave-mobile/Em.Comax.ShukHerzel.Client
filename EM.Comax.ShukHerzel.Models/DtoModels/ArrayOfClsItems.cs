using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
    [XmlRoot(Namespace = "http://ws.comax.co.il/Comax_WebServices/", IsNullable = false)]
    public class ArrayOfClsItems
    {
        [XmlElement("ClsItems")]
        public List<ArrayOfClsItemsClsItems>? ClsItems { get; set; }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
    public class ArrayOfClsItemsClsItems
    {
        public string Name { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public long ID { get; set; } = 0;
        public long OldID { get; set; } = 0;
        public string? AnotherBarkods { get; set; }
        public string? WebShortName { get; set; }
        public string? WebDesc { get; set; }
        public string? WebDesc2 { get; set; }
        public string? WebDelvDesc { get; set; }
        public string? WebInstruction { get; set; }
        public long Barcode { get; set; } = 0;
        public long PurchasingAccountID { get; set; } = 0;
        public long SalesAccountID { get; set; } = 0;
        public string? AlternateID { get; set; }
        public bool WorkByAlternateID { get; set; } = false;
        public string? SerialNo { get; set; }
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
        public string? DepartmentEnglishName { get; set; }
        public string Group { get; set; } = string.Empty;
        public long GroupCode { get; set; } = 0;
        public string? GroupEnglishName { get; set; }
        public string Sub_Group { get; set; } = string.Empty;
        public long Sub_GroupCode { get; set; } = 0;
        public string? Sub_GroupEnglishName { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public long ManufacturerCode { get; set; } = 0;
        public ItemModel? ItemModel { get; set; }
        public ItemColor? ItemColor { get; set; }
        public ItemSize? ItemSize { get; set; }
        public ItemsTree? ItemsTree { get; set; }
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
        public string? SupplierCatalogNumber { get; set; }
        public decimal SupplierPrice { get; set; } = 0;
        public string? SupplierCurrency { get; set; }
        public long SupplierShekelPrice { get; set; } = 0;
        public decimal SupplierPriceNet { get; set; } = 0;
        public decimal SupplierShekelPriceNet { get; set; } = 0;
        public string? ArchiveDate { get; set; }
        public string? BlockPurchaseDate { get; set; }
        public string? BlockSalesDate { get; set; }
        public string? EnglishName { get; set; }
        public long PromoID { get; set; } = 0;
        public long PromoRank { get; set; } = 0;
        public long MatchingItemID { get; set; } = 0;
        public long MatchingItemQuantity { get; set; } = 0;
        public long UnitsQuantity { get; set; } = 0;
        public long Length { get; set; } = 0;
        public string? LengthSize { get; set; }
        public long Width { get; set; } = 0;
        public string? WidthSize { get; set; }
        public long Height { get; set; } = 0;
        public string? HeightSize { get; set; }
        public long Weight { get; set; } = 0;
        public string? WeightSize { get; set; }
        public long CurrentStoreID { get; set; } = 0;
        public long BaseLine { get; set; } = 0;
        public long BaseDocNumber { get; set; } = 0;
        public long BaseReference { get; set; } = 0;
        public long BaseYear { get; set; } = 0;
        public long Conversion { get; set; } = 0;
        public long AdditionalConversion { get; set; } = 0;
        public bool NotShowInWeb { get; set; } = false;
        public string? LinkWebSite { get; set; }
        public bool SetModelProperties { get; set; } = false;
        public bool HierarchyUpdate { get; set; } = false;
        public string DownloadPriceTransparencyAfter48Hours { get; set; } = string.Empty;
        public string ItemData { get; set; } = string.Empty;
        public long RejectTypeID { get; set; } = 0;
        public string OpenDate { get; set; } = string.Empty;
        public long WarrantyMonths { get; set; } = 0;
        public string? Ingredients { get; set; }
        public bool FullIngredientsReplace { get; set; } = false;
        public decimal CmtAmara { get; set; } = 0;
        public string AmaraBy { get; set; } = string.Empty;
        public decimal CmtAmara2 { get; set; } = 0;
        public decimal InternalAmara { get; set; } = 0;
        public long Line { get; set; } = 0;
        public decimal LengthOptic { get; set; } = 0;
        public decimal DiagonalOptic { get; set; } = 0;
        public decimal HeightOptic { get; set; } = 0;
        public string? NameInWeb { get; set; }
        public string? ManufacturingCountry { get; set; }
        public decimal? Content { get; set; }
        public string? ContentUnit { get; set; }
        public decimal? ContentMeasure { get; set; }
        public string? SwPikadon { get; set; }
        public string? TrailingItem { get; set; }
    }

    public class ItemModel
    {
        public string Name { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty;
    }

    public class ItemColor
    {
        public string? Name { get; set; }
        public string? ID { get; set; }
    }

    public class ItemSize
    {
        public string? Name { get; set; }
        public string? ID { get; set; }
    }     
    
    public class ItemsTree
    {
        [XmlElement("ClsItemTree")]
        public List<ClsItemTree> ClsItemTree { get; set; }
         
    }

    public class ClsItemTree
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("ID")]
        public string ID { get; set; }
    }
}
