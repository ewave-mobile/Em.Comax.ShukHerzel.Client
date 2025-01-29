using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    public class esldto_
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ws.comax.co.il/Comax_WebServices/", IsNullable = false)]
        public partial class ArrayOfClsItems
        {

            private ArrayOfClsItemsClsItems[] clsItemsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("ClsItems")]
            public ArrayOfClsItemsClsItems[] ClsItems
            {
                get
                {
                    return this.clsItemsField;
                }
                set
                {
                    this.clsItemsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
        public partial class ArrayOfClsItemsClsItems
        {

            private string nameField;

            private string sizeField;

            private ulong idField;

            private byte oldIDField;

            private object anotherBarkodsField;

            private object webShortNameField;

            private object webDescField;

            private object webDesc2Field;

            private object webDelvDescField;

            private object webInstructionField;

            private ulong barcodeField;

            private byte purchasingAccountIDField;

            private byte salesAccountIDField;

            private object alternateIDField;

            private bool workByAlternateIDField;

            private object serialNoField;

            private decimal priceField;

            private decimal quantityField;

            private byte discountPercentField;

            private byte discountTotalField;

            private byte totalSumField;

            private byte maxQuantityInOrderField;

            private byte correctionPriceField;

            private byte correctionDiscountPercentField;

            private byte correctionQuantityField;

            private byte conversionQuantityField;

            private string superDepartmentField;

            private byte superDepartmentCodeField;

            private string departmentField;

            private ushort departmentCodeField;

            private object departmentEnglishNameField;

            private string groupField;

            private ushort groupCodeField;

            private object groupEnglishNameField;

            private string sub_GroupField;

            private ushort sub_GroupCodeField;

            private object sub_GroupEnglishNameField;

            private string manufacturerField;

            private byte manufacturerCodeField;

            private ArrayOfClsItemsClsItemsItemModel itemModelField;

            private ArrayOfClsItemsClsItemsItemColor itemColorField;

            private ArrayOfClsItemsClsItemsItemSize itemSizeField;

            private object itemsTreeField;

            private string attribute1Field;

            private string attribute1CodeField;

            private object attribute2Field;

            private object attribute2CodeField;

            private object attribute3Field;

            private object attribute3CodeField;

            private object attribute4Field;

            private object attribute4CodeField;

            private object attribute5Field;

            private object attribute5CodeField;

            private uint supplierIDField;

            private string supplierNameField;

            private object supplierCatalogNumberField;

            private decimal supplierPriceField;

            private object supplierCurrencyField;

            private byte supplierShekelPriceField;

            private decimal supplierPriceNetField;

            private decimal supplierShekelPriceNetField;

            private object archiveDateField;

            private object blockPurchaseDateField;

            private object blockSalesDateField;

            private object englishNameField;

            private byte promoIDField;

            private byte promoRankField;

            private byte matchingItemIDField;

            private byte matchingItemQuantityField;

            private byte unitsQuantityField;

            private byte lengthField;

            private object lengthSizeField;

            private byte widthField;

            private object widthSizeField;

            private byte heightField;

            private object heightSizeField;

            private byte weightField;

            private object weightSizeField;

            private byte currentStoreIDField;

            private byte baseLineField;

            private byte baseDocNumberField;

            private byte baseReferenceField;

            private byte baseYearField;

            private byte conversionField;

            private byte additionalConversionField;

            private bool notShowInWebField;

            private object linkWebSiteField;

            private bool setModelPropertiesField;

            private bool hierarchyUpdateField;

            private string downloadPriceTransparencyAfter48HoursField;

            private string itemDataField;

            private byte rejectTypeIDField;

            private string openDateField;

            private byte warrantyMonthsField;

            private object ingredientsField;

            private bool fullIngredientsReplaceField;

            private byte cmtAmaraField;

            private string amaraByField;

            private byte cmtAmara2Field;

            private byte internalAmaraField;

            private byte lineField;

            private byte lengthOpticField;

            private byte diagonalOpticField;

            private byte heightOpticField;

            private object nameInWebField;

            private object nosafKod_2Field;

            private object nosafNm_2Field;

            private object nosafKod_3Field;

            private object nosafNm_3Field;

            private object atarDescriptionField;

            private byte energiaField;

            private byte pahmemotField;

            private byte helbonimField;

            private byte shumanimField;

            private byte colestrolField;

            private byte mhrMivzaField;

            private bool swBonosField;

            private byte bonosFlineField;

            private byte bonosTlineField;

            private byte mirsham_PhrmaSoftField;

            private byte customerMakatField;

            private string qtyTypeField;

            private byte noDiscountField;

            private byte priceUSDField;

            private byte priceNISField;

            private byte warrantySupplierField;

            private object warrantyRemarkField;

            private ushort contentField;

            private string contentUnitField;

            private byte contentMeasureField;

            private byte volumeField;

            private object volumeSizeField;

            private bool swWeighableField;

            private byte radiusField;

            private byte navaField;

            private byte thicknessField;

            private byte minStockField;

            private byte maxStockField;

            private byte suppliersOrdersField;

            private byte customersOrdersField;

            private bool swSerialyField;

            private byte storeIDField;

            private decimal netPriceField;

            private byte cmtAmr3Field;

            private object midaAmr3Field;

            private object nm_MashlimField;

            private object webManufacturerField;

            private byte webManufacturerCodeField;

            private object webCategoryField;

            private byte webSub_GroupField;

            private byte webColorField;

            private byte webDepField;

            private byte webGrpField;

            private object webNosafKod_2Field;

            private object webNosafNm_2Field;

            private bool isGiftCardPurchaseField;

            private object webColorNmField;

            private object webDepNmField;

            private object webGrpNmField;

            private object webSub_GroupNmField;

            private object webCategoryNmField;

            private byte webRatingField;

            private byte limitAgeField;

            private byte swPikadonField;

            private byte trailingItemField;

            private byte kodSetField;

            private byte setIdField;

            private byte minimumStockField;

            private byte maximumStockField;

            private byte stiaAMField;

            private object nosafKod_4Field;

            private object nosafNm_4Field;

            private object nosafKod_5Field;

            private object nosafNm_5Field;

            private object nosafKod_6Field;

            private object nosafNm_6Field;

            private object nosafKod_7Field;

            private object nosafNm_7Field;

            private object nosafKod_8Field;

            private object nosafNm_8Field;

            private object nosafKod_9Field;

            private object nosafNm_9Field;

            private object nosafKod_10Field;

            private object nosafNm_10Field;

            private byte swAlcoholField;

            private byte swMustCmtInKupaField;

            private string manufacturingCountryField;

            private byte pikadonCmtField;

            private byte isControlledItemField;

            private byte minQtyForSaleField;

            private byte maxQtyForSaleField;

            private uint prtCField;

            private object colorGrpField;

            private byte ref_LineField;

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string Size
            {
                get
                {
                    return this.sizeField;
                }
                set
                {
                    this.sizeField = value;
                }
            }

            /// <remarks/>
            public ulong ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public byte OldID
            {
                get
                {
                    return this.oldIDField;
                }
                set
                {
                    this.oldIDField = value;
                }
            }

            /// <remarks/>
            public object AnotherBarkods
            {
                get
                {
                    return this.anotherBarkodsField;
                }
                set
                {
                    this.anotherBarkodsField = value;
                }
            }

            /// <remarks/>
            public object WebShortName
            {
                get
                {
                    return this.webShortNameField;
                }
                set
                {
                    this.webShortNameField = value;
                }
            }

            /// <remarks/>
            public object WebDesc
            {
                get
                {
                    return this.webDescField;
                }
                set
                {
                    this.webDescField = value;
                }
            }

            /// <remarks/>
            public object WebDesc2
            {
                get
                {
                    return this.webDesc2Field;
                }
                set
                {
                    this.webDesc2Field = value;
                }
            }

            /// <remarks/>
            public object WebDelvDesc
            {
                get
                {
                    return this.webDelvDescField;
                }
                set
                {
                    this.webDelvDescField = value;
                }
            }

            /// <remarks/>
            public object WebInstruction
            {
                get
                {
                    return this.webInstructionField;
                }
                set
                {
                    this.webInstructionField = value;
                }
            }

            /// <remarks/>
            public ulong Barcode
            {
                get
                {
                    return this.barcodeField;
                }
                set
                {
                    this.barcodeField = value;
                }
            }

            /// <remarks/>
            public byte PurchasingAccountID
            {
                get
                {
                    return this.purchasingAccountIDField;
                }
                set
                {
                    this.purchasingAccountIDField = value;
                }
            }

            /// <remarks/>
            public byte SalesAccountID
            {
                get
                {
                    return this.salesAccountIDField;
                }
                set
                {
                    this.salesAccountIDField = value;
                }
            }

            /// <remarks/>
            public object AlternateID
            {
                get
                {
                    return this.alternateIDField;
                }
                set
                {
                    this.alternateIDField = value;
                }
            }

            /// <remarks/>
            public bool WorkByAlternateID
            {
                get
                {
                    return this.workByAlternateIDField;
                }
                set
                {
                    this.workByAlternateIDField = value;
                }
            }

            /// <remarks/>
            public object SerialNo
            {
                get
                {
                    return this.serialNoField;
                }
                set
                {
                    this.serialNoField = value;
                }
            }

            /// <remarks/>
            public decimal Price
            {
                get
                {
                    return this.priceField;
                }
                set
                {
                    this.priceField = value;
                }
            }

            /// <remarks/>
            public decimal Quantity
            {
                get
                {
                    return this.quantityField;
                }
                set
                {
                    this.quantityField = value;
                }
            }

            /// <remarks/>
            public byte DiscountPercent
            {
                get
                {
                    return this.discountPercentField;
                }
                set
                {
                    this.discountPercentField = value;
                }
            }

            /// <remarks/>
            public byte DiscountTotal
            {
                get
                {
                    return this.discountTotalField;
                }
                set
                {
                    this.discountTotalField = value;
                }
            }

            /// <remarks/>
            public byte TotalSum
            {
                get
                {
                    return this.totalSumField;
                }
                set
                {
                    this.totalSumField = value;
                }
            }

            /// <remarks/>
            public byte MaxQuantityInOrder
            {
                get
                {
                    return this.maxQuantityInOrderField;
                }
                set
                {
                    this.maxQuantityInOrderField = value;
                }
            }

            /// <remarks/>
            public byte CorrectionPrice
            {
                get
                {
                    return this.correctionPriceField;
                }
                set
                {
                    this.correctionPriceField = value;
                }
            }

            /// <remarks/>
            public byte CorrectionDiscountPercent
            {
                get
                {
                    return this.correctionDiscountPercentField;
                }
                set
                {
                    this.correctionDiscountPercentField = value;
                }
            }

            /// <remarks/>
            public byte CorrectionQuantity
            {
                get
                {
                    return this.correctionQuantityField;
                }
                set
                {
                    this.correctionQuantityField = value;
                }
            }

            /// <remarks/>
            public byte ConversionQuantity
            {
                get
                {
                    return this.conversionQuantityField;
                }
                set
                {
                    this.conversionQuantityField = value;
                }
            }

            /// <remarks/>
            public string SuperDepartment
            {
                get
                {
                    return this.superDepartmentField;
                }
                set
                {
                    this.superDepartmentField = value;
                }
            }

            /// <remarks/>
            public byte SuperDepartmentCode
            {
                get
                {
                    return this.superDepartmentCodeField;
                }
                set
                {
                    this.superDepartmentCodeField = value;
                }
            }

            /// <remarks/>
            public string Department
            {
                get
                {
                    return this.departmentField;
                }
                set
                {
                    this.departmentField = value;
                }
            }

            /// <remarks/>
            public ushort DepartmentCode
            {
                get
                {
                    return this.departmentCodeField;
                }
                set
                {
                    this.departmentCodeField = value;
                }
            }

            /// <remarks/>
            public object DepartmentEnglishName
            {
                get
                {
                    return this.departmentEnglishNameField;
                }
                set
                {
                    this.departmentEnglishNameField = value;
                }
            }

            /// <remarks/>
            public string Group
            {
                get
                {
                    return this.groupField;
                }
                set
                {
                    this.groupField = value;
                }
            }

            /// <remarks/>
            public ushort GroupCode
            {
                get
                {
                    return this.groupCodeField;
                }
                set
                {
                    this.groupCodeField = value;
                }
            }

            /// <remarks/>
            public object GroupEnglishName
            {
                get
                {
                    return this.groupEnglishNameField;
                }
                set
                {
                    this.groupEnglishNameField = value;
                }
            }

            /// <remarks/>
            public string Sub_Group
            {
                get
                {
                    return this.sub_GroupField;
                }
                set
                {
                    this.sub_GroupField = value;
                }
            }

            /// <remarks/>
            public ushort Sub_GroupCode
            {
                get
                {
                    return this.sub_GroupCodeField;
                }
                set
                {
                    this.sub_GroupCodeField = value;
                }
            }

            /// <remarks/>
            public object Sub_GroupEnglishName
            {
                get
                {
                    return this.sub_GroupEnglishNameField;
                }
                set
                {
                    this.sub_GroupEnglishNameField = value;
                }
            }

            /// <remarks/>
            public string Manufacturer
            {
                get
                {
                    return this.manufacturerField;
                }
                set
                {
                    this.manufacturerField = value;
                }
            }

            /// <remarks/>
            public byte ManufacturerCode
            {
                get
                {
                    return this.manufacturerCodeField;
                }
                set
                {
                    this.manufacturerCodeField = value;
                }
            }

            /// <remarks/>
            public ArrayOfClsItemsClsItemsItemModel ItemModel
            {
                get
                {
                    return this.itemModelField;
                }
                set
                {
                    this.itemModelField = value;
                }
            }

            /// <remarks/>
            public ArrayOfClsItemsClsItemsItemColor ItemColor
            {
                get
                {
                    return this.itemColorField;
                }
                set
                {
                    this.itemColorField = value;
                }
            }

            /// <remarks/>
            public ArrayOfClsItemsClsItemsItemSize ItemSize
            {
                get
                {
                    return this.itemSizeField;
                }
                set
                {
                    this.itemSizeField = value;
                }
            }

            /// <remarks/>
            public object ItemsTree
            {
                get
                {
                    return this.itemsTreeField;
                }
                set
                {
                    this.itemsTreeField = value;
                }
            }

            /// <remarks/>
            public string Attribute1
            {
                get
                {
                    return this.attribute1Field;
                }
                set
                {
                    this.attribute1Field = value;
                }
            }

            /// <remarks/>
            public string Attribute1Code
            {
                get
                {
                    return this.attribute1CodeField;
                }
                set
                {
                    this.attribute1CodeField = value;
                }
            }

            /// <remarks/>
            public object Attribute2
            {
                get
                {
                    return this.attribute2Field;
                }
                set
                {
                    this.attribute2Field = value;
                }
            }

            /// <remarks/>
            public object Attribute2Code
            {
                get
                {
                    return this.attribute2CodeField;
                }
                set
                {
                    this.attribute2CodeField = value;
                }
            }

            /// <remarks/>
            public object Attribute3
            {
                get
                {
                    return this.attribute3Field;
                }
                set
                {
                    this.attribute3Field = value;
                }
            }

            /// <remarks/>
            public object Attribute3Code
            {
                get
                {
                    return this.attribute3CodeField;
                }
                set
                {
                    this.attribute3CodeField = value;
                }
            }

            /// <remarks/>
            public object Attribute4
            {
                get
                {
                    return this.attribute4Field;
                }
                set
                {
                    this.attribute4Field = value;
                }
            }

            /// <remarks/>
            public object Attribute4Code
            {
                get
                {
                    return this.attribute4CodeField;
                }
                set
                {
                    this.attribute4CodeField = value;
                }
            }

            /// <remarks/>
            public object Attribute5
            {
                get
                {
                    return this.attribute5Field;
                }
                set
                {
                    this.attribute5Field = value;
                }
            }

            /// <remarks/>
            public object Attribute5Code
            {
                get
                {
                    return this.attribute5CodeField;
                }
                set
                {
                    this.attribute5CodeField = value;
                }
            }

            /// <remarks/>
            public uint SupplierID
            {
                get
                {
                    return this.supplierIDField;
                }
                set
                {
                    this.supplierIDField = value;
                }
            }

            /// <remarks/>
            public string SupplierName
            {
                get
                {
                    return this.supplierNameField;
                }
                set
                {
                    this.supplierNameField = value;
                }
            }

            /// <remarks/>
            public object SupplierCatalogNumber
            {
                get
                {
                    return this.supplierCatalogNumberField;
                }
                set
                {
                    this.supplierCatalogNumberField = value;
                }
            }

            /// <remarks/>
            public decimal SupplierPrice
            {
                get
                {
                    return this.supplierPriceField;
                }
                set
                {
                    this.supplierPriceField = value;
                }
            }

            /// <remarks/>
            public object SupplierCurrency
            {
                get
                {
                    return this.supplierCurrencyField;
                }
                set
                {
                    this.supplierCurrencyField = value;
                }
            }

            /// <remarks/>
            public byte SupplierShekelPrice
            {
                get
                {
                    return this.supplierShekelPriceField;
                }
                set
                {
                    this.supplierShekelPriceField = value;
                }
            }

            /// <remarks/>
            public decimal SupplierPriceNet
            {
                get
                {
                    return this.supplierPriceNetField;
                }
                set
                {
                    this.supplierPriceNetField = value;
                }
            }

            /// <remarks/>
            public decimal SupplierShekelPriceNet
            {
                get
                {
                    return this.supplierShekelPriceNetField;
                }
                set
                {
                    this.supplierShekelPriceNetField = value;
                }
            }

            /// <remarks/>
            public object ArchiveDate
            {
                get
                {
                    return this.archiveDateField;
                }
                set
                {
                    this.archiveDateField = value;
                }
            }

            /// <remarks/>
            public object BlockPurchaseDate
            {
                get
                {
                    return this.blockPurchaseDateField;
                }
                set
                {
                    this.blockPurchaseDateField = value;
                }
            }

            /// <remarks/>
            public object BlockSalesDate
            {
                get
                {
                    return this.blockSalesDateField;
                }
                set
                {
                    this.blockSalesDateField = value;
                }
            }

            /// <remarks/>
            public object EnglishName
            {
                get
                {
                    return this.englishNameField;
                }
                set
                {
                    this.englishNameField = value;
                }
            }

            /// <remarks/>
            public byte PromoID
            {
                get
                {
                    return this.promoIDField;
                }
                set
                {
                    this.promoIDField = value;
                }
            }

            /// <remarks/>
            public byte PromoRank
            {
                get
                {
                    return this.promoRankField;
                }
                set
                {
                    this.promoRankField = value;
                }
            }

            /// <remarks/>
            public byte MatchingItemID
            {
                get
                {
                    return this.matchingItemIDField;
                }
                set
                {
                    this.matchingItemIDField = value;
                }
            }

            /// <remarks/>
            public byte MatchingItemQuantity
            {
                get
                {
                    return this.matchingItemQuantityField;
                }
                set
                {
                    this.matchingItemQuantityField = value;
                }
            }

            /// <remarks/>
            public byte UnitsQuantity
            {
                get
                {
                    return this.unitsQuantityField;
                }
                set
                {
                    this.unitsQuantityField = value;
                }
            }

            /// <remarks/>
            public byte Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            public object LengthSize
            {
                get
                {
                    return this.lengthSizeField;
                }
                set
                {
                    this.lengthSizeField = value;
                }
            }

            /// <remarks/>
            public byte Width
            {
                get
                {
                    return this.widthField;
                }
                set
                {
                    this.widthField = value;
                }
            }

            /// <remarks/>
            public object WidthSize
            {
                get
                {
                    return this.widthSizeField;
                }
                set
                {
                    this.widthSizeField = value;
                }
            }

            /// <remarks/>
            public byte Height
            {
                get
                {
                    return this.heightField;
                }
                set
                {
                    this.heightField = value;
                }
            }

            /// <remarks/>
            public object HeightSize
            {
                get
                {
                    return this.heightSizeField;
                }
                set
                {
                    this.heightSizeField = value;
                }
            }

            /// <remarks/>
            public byte Weight
            {
                get
                {
                    return this.weightField;
                }
                set
                {
                    this.weightField = value;
                }
            }

            /// <remarks/>
            public object WeightSize
            {
                get
                {
                    return this.weightSizeField;
                }
                set
                {
                    this.weightSizeField = value;
                }
            }

            /// <remarks/>
            public byte CurrentStoreID
            {
                get
                {
                    return this.currentStoreIDField;
                }
                set
                {
                    this.currentStoreIDField = value;
                }
            }

            /// <remarks/>
            public byte BaseLine
            {
                get
                {
                    return this.baseLineField;
                }
                set
                {
                    this.baseLineField = value;
                }
            }

            /// <remarks/>
            public byte BaseDocNumber
            {
                get
                {
                    return this.baseDocNumberField;
                }
                set
                {
                    this.baseDocNumberField = value;
                }
            }

            /// <remarks/>
            public byte BaseReference
            {
                get
                {
                    return this.baseReferenceField;
                }
                set
                {
                    this.baseReferenceField = value;
                }
            }

            /// <remarks/>
            public byte BaseYear
            {
                get
                {
                    return this.baseYearField;
                }
                set
                {
                    this.baseYearField = value;
                }
            }

            /// <remarks/>
            public byte Conversion
            {
                get
                {
                    return this.conversionField;
                }
                set
                {
                    this.conversionField = value;
                }
            }

            /// <remarks/>
            public byte AdditionalConversion
            {
                get
                {
                    return this.additionalConversionField;
                }
                set
                {
                    this.additionalConversionField = value;
                }
            }

            /// <remarks/>
            public bool NotShowInWeb
            {
                get
                {
                    return this.notShowInWebField;
                }
                set
                {
                    this.notShowInWebField = value;
                }
            }

            /// <remarks/>
            public object LinkWebSite
            {
                get
                {
                    return this.linkWebSiteField;
                }
                set
                {
                    this.linkWebSiteField = value;
                }
            }

            /// <remarks/>
            public bool SetModelProperties
            {
                get
                {
                    return this.setModelPropertiesField;
                }
                set
                {
                    this.setModelPropertiesField = value;
                }
            }

            /// <remarks/>
            public bool HierarchyUpdate
            {
                get
                {
                    return this.hierarchyUpdateField;
                }
                set
                {
                    this.hierarchyUpdateField = value;
                }
            }

            /// <remarks/>
            public string DownloadPriceTransparencyAfter48Hours
            {
                get
                {
                    return this.downloadPriceTransparencyAfter48HoursField;
                }
                set
                {
                    this.downloadPriceTransparencyAfter48HoursField = value;
                }
            }

            /// <remarks/>
            public string ItemData
            {
                get
                {
                    return this.itemDataField;
                }
                set
                {
                    this.itemDataField = value;
                }
            }

            /// <remarks/>
            public byte RejectTypeID
            {
                get
                {
                    return this.rejectTypeIDField;
                }
                set
                {
                    this.rejectTypeIDField = value;
                }
            }

            /// <remarks/>
            public string OpenDate
            {
                get
                {
                    return this.openDateField;
                }
                set
                {
                    this.openDateField = value;
                }
            }

            /// <remarks/>
            public byte WarrantyMonths
            {
                get
                {
                    return this.warrantyMonthsField;
                }
                set
                {
                    this.warrantyMonthsField = value;
                }
            }

            /// <remarks/>
            public object Ingredients
            {
                get
                {
                    return this.ingredientsField;
                }
                set
                {
                    this.ingredientsField = value;
                }
            }

            /// <remarks/>
            public bool FullIngredientsReplace
            {
                get
                {
                    return this.fullIngredientsReplaceField;
                }
                set
                {
                    this.fullIngredientsReplaceField = value;
                }
            }

            /// <remarks/>
            public byte CmtAmara
            {
                get
                {
                    return this.cmtAmaraField;
                }
                set
                {
                    this.cmtAmaraField = value;
                }
            }

            /// <remarks/>
            public string AmaraBy
            {
                get
                {
                    return this.amaraByField;
                }
                set
                {
                    this.amaraByField = value;
                }
            }

            /// <remarks/>
            public byte CmtAmara2
            {
                get
                {
                    return this.cmtAmara2Field;
                }
                set
                {
                    this.cmtAmara2Field = value;
                }
            }

            /// <remarks/>
            public byte InternalAmara
            {
                get
                {
                    return this.internalAmaraField;
                }
                set
                {
                    this.internalAmaraField = value;
                }
            }

            /// <remarks/>
            public byte Line
            {
                get
                {
                    return this.lineField;
                }
                set
                {
                    this.lineField = value;
                }
            }

            /// <remarks/>
            public byte LengthOptic
            {
                get
                {
                    return this.lengthOpticField;
                }
                set
                {
                    this.lengthOpticField = value;
                }
            }

            /// <remarks/>
            public byte DiagonalOptic
            {
                get
                {
                    return this.diagonalOpticField;
                }
                set
                {
                    this.diagonalOpticField = value;
                }
            }

            /// <remarks/>
            public byte HeightOptic
            {
                get
                {
                    return this.heightOpticField;
                }
                set
                {
                    this.heightOpticField = value;
                }
            }

            /// <remarks/>
            public object NameInWeb
            {
                get
                {
                    return this.nameInWebField;
                }
                set
                {
                    this.nameInWebField = value;
                }
            }

            /// <remarks/>
            public object NosafKod_2
            {
                get
                {
                    return this.nosafKod_2Field;
                }
                set
                {
                    this.nosafKod_2Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_2
            {
                get
                {
                    return this.nosafNm_2Field;
                }
                set
                {
                    this.nosafNm_2Field = value;
                }
            }

            /// <remarks/>
            public object NosafKod_3
            {
                get
                {
                    return this.nosafKod_3Field;
                }
                set
                {
                    this.nosafKod_3Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_3
            {
                get
                {
                    return this.nosafNm_3Field;
                }
                set
                {
                    this.nosafNm_3Field = value;
                }
            }

            /// <remarks/>
            public object AtarDescription
            {
                get
                {
                    return this.atarDescriptionField;
                }
                set
                {
                    this.atarDescriptionField = value;
                }
            }

            /// <remarks/>
            public byte Energia
            {
                get
                {
                    return this.energiaField;
                }
                set
                {
                    this.energiaField = value;
                }
            }

            /// <remarks/>
            public byte Pahmemot
            {
                get
                {
                    return this.pahmemotField;
                }
                set
                {
                    this.pahmemotField = value;
                }
            }

            /// <remarks/>
            public byte Helbonim
            {
                get
                {
                    return this.helbonimField;
                }
                set
                {
                    this.helbonimField = value;
                }
            }

            /// <remarks/>
            public byte Shumanim
            {
                get
                {
                    return this.shumanimField;
                }
                set
                {
                    this.shumanimField = value;
                }
            }

            /// <remarks/>
            public byte Colestrol
            {
                get
                {
                    return this.colestrolField;
                }
                set
                {
                    this.colestrolField = value;
                }
            }

            /// <remarks/>
            public byte MhrMivza
            {
                get
                {
                    return this.mhrMivzaField;
                }
                set
                {
                    this.mhrMivzaField = value;
                }
            }

            /// <remarks/>
            public bool SwBonos
            {
                get
                {
                    return this.swBonosField;
                }
                set
                {
                    this.swBonosField = value;
                }
            }

            /// <remarks/>
            public byte BonosFline
            {
                get
                {
                    return this.bonosFlineField;
                }
                set
                {
                    this.bonosFlineField = value;
                }
            }

            /// <remarks/>
            public byte BonosTline
            {
                get
                {
                    return this.bonosTlineField;
                }
                set
                {
                    this.bonosTlineField = value;
                }
            }

            /// <remarks/>
            public byte Mirsham_PhrmaSoft
            {
                get
                {
                    return this.mirsham_PhrmaSoftField;
                }
                set
                {
                    this.mirsham_PhrmaSoftField = value;
                }
            }

            /// <remarks/>
            public byte CustomerMakat
            {
                get
                {
                    return this.customerMakatField;
                }
                set
                {
                    this.customerMakatField = value;
                }
            }

            /// <remarks/>
            public string QtyType
            {
                get
                {
                    return this.qtyTypeField;
                }
                set
                {
                    this.qtyTypeField = value;
                }
            }

            /// <remarks/>
            public byte NoDiscount
            {
                get
                {
                    return this.noDiscountField;
                }
                set
                {
                    this.noDiscountField = value;
                }
            }

            /// <remarks/>
            public byte PriceUSD
            {
                get
                {
                    return this.priceUSDField;
                }
                set
                {
                    this.priceUSDField = value;
                }
            }

            /// <remarks/>
            public byte PriceNIS
            {
                get
                {
                    return this.priceNISField;
                }
                set
                {
                    this.priceNISField = value;
                }
            }

            /// <remarks/>
            public byte WarrantySupplier
            {
                get
                {
                    return this.warrantySupplierField;
                }
                set
                {
                    this.warrantySupplierField = value;
                }
            }

            /// <remarks/>
            public object WarrantyRemark
            {
                get
                {
                    return this.warrantyRemarkField;
                }
                set
                {
                    this.warrantyRemarkField = value;
                }
            }

            /// <remarks/>
            public ushort Content
            {
                get
                {
                    return this.contentField;
                }
                set
                {
                    this.contentField = value;
                }
            }

            /// <remarks/>
            public string ContentUnit
            {
                get
                {
                    return this.contentUnitField;
                }
                set
                {
                    this.contentUnitField = value;
                }
            }

            /// <remarks/>
            public byte ContentMeasure
            {
                get
                {
                    return this.contentMeasureField;
                }
                set
                {
                    this.contentMeasureField = value;
                }
            }

            /// <remarks/>
            public byte Volume
            {
                get
                {
                    return this.volumeField;
                }
                set
                {
                    this.volumeField = value;
                }
            }

            /// <remarks/>
            public object VolumeSize
            {
                get
                {
                    return this.volumeSizeField;
                }
                set
                {
                    this.volumeSizeField = value;
                }
            }

            /// <remarks/>
            public bool SwWeighable
            {
                get
                {
                    return this.swWeighableField;
                }
                set
                {
                    this.swWeighableField = value;
                }
            }

            /// <remarks/>
            public byte Radius
            {
                get
                {
                    return this.radiusField;
                }
                set
                {
                    this.radiusField = value;
                }
            }

            /// <remarks/>
            public byte Nava
            {
                get
                {
                    return this.navaField;
                }
                set
                {
                    this.navaField = value;
                }
            }

            /// <remarks/>
            public byte Thickness
            {
                get
                {
                    return this.thicknessField;
                }
                set
                {
                    this.thicknessField = value;
                }
            }

            /// <remarks/>
            public byte MinStock
            {
                get
                {
                    return this.minStockField;
                }
                set
                {
                    this.minStockField = value;
                }
            }

            /// <remarks/>
            public byte MaxStock
            {
                get
                {
                    return this.maxStockField;
                }
                set
                {
                    this.maxStockField = value;
                }
            }

            /// <remarks/>
            public byte SuppliersOrders
            {
                get
                {
                    return this.suppliersOrdersField;
                }
                set
                {
                    this.suppliersOrdersField = value;
                }
            }

            /// <remarks/>
            public byte CustomersOrders
            {
                get
                {
                    return this.customersOrdersField;
                }
                set
                {
                    this.customersOrdersField = value;
                }
            }

            /// <remarks/>
            public bool SwSerialy
            {
                get
                {
                    return this.swSerialyField;
                }
                set
                {
                    this.swSerialyField = value;
                }
            }

            /// <remarks/>
            public byte StoreID
            {
                get
                {
                    return this.storeIDField;
                }
                set
                {
                    this.storeIDField = value;
                }
            }

            /// <remarks/>
            public decimal NetPrice
            {
                get
                {
                    return this.netPriceField;
                }
                set
                {
                    this.netPriceField = value;
                }
            }

            /// <remarks/>
            public byte CmtAmr3
            {
                get
                {
                    return this.cmtAmr3Field;
                }
                set
                {
                    this.cmtAmr3Field = value;
                }
            }

            /// <remarks/>
            public object MidaAmr3
            {
                get
                {
                    return this.midaAmr3Field;
                }
                set
                {
                    this.midaAmr3Field = value;
                }
            }

            /// <remarks/>
            public object Nm_Mashlim
            {
                get
                {
                    return this.nm_MashlimField;
                }
                set
                {
                    this.nm_MashlimField = value;
                }
            }

            /// <remarks/>
            public object WebManufacturer
            {
                get
                {
                    return this.webManufacturerField;
                }
                set
                {
                    this.webManufacturerField = value;
                }
            }

            /// <remarks/>
            public byte WebManufacturerCode
            {
                get
                {
                    return this.webManufacturerCodeField;
                }
                set
                {
                    this.webManufacturerCodeField = value;
                }
            }

            /// <remarks/>
            public object WebCategory
            {
                get
                {
                    return this.webCategoryField;
                }
                set
                {
                    this.webCategoryField = value;
                }
            }

            /// <remarks/>
            public byte WebSub_Group
            {
                get
                {
                    return this.webSub_GroupField;
                }
                set
                {
                    this.webSub_GroupField = value;
                }
            }

            /// <remarks/>
            public byte WebColor
            {
                get
                {
                    return this.webColorField;
                }
                set
                {
                    this.webColorField = value;
                }
            }

            /// <remarks/>
            public byte WebDep
            {
                get
                {
                    return this.webDepField;
                }
                set
                {
                    this.webDepField = value;
                }
            }

            /// <remarks/>
            public byte WebGrp
            {
                get
                {
                    return this.webGrpField;
                }
                set
                {
                    this.webGrpField = value;
                }
            }

            /// <remarks/>
            public object WebNosafKod_2
            {
                get
                {
                    return this.webNosafKod_2Field;
                }
                set
                {
                    this.webNosafKod_2Field = value;
                }
            }

            /// <remarks/>
            public object WebNosafNm_2
            {
                get
                {
                    return this.webNosafNm_2Field;
                }
                set
                {
                    this.webNosafNm_2Field = value;
                }
            }

            /// <remarks/>
            public bool IsGiftCardPurchase
            {
                get
                {
                    return this.isGiftCardPurchaseField;
                }
                set
                {
                    this.isGiftCardPurchaseField = value;
                }
            }

            /// <remarks/>
            public object WebColorNm
            {
                get
                {
                    return this.webColorNmField;
                }
                set
                {
                    this.webColorNmField = value;
                }
            }

            /// <remarks/>
            public object WebDepNm
            {
                get
                {
                    return this.webDepNmField;
                }
                set
                {
                    this.webDepNmField = value;
                }
            }

            /// <remarks/>
            public object WebGrpNm
            {
                get
                {
                    return this.webGrpNmField;
                }
                set
                {
                    this.webGrpNmField = value;
                }
            }

            /// <remarks/>
            public object WebSub_GroupNm
            {
                get
                {
                    return this.webSub_GroupNmField;
                }
                set
                {
                    this.webSub_GroupNmField = value;
                }
            }

            /// <remarks/>
            public object WebCategoryNm
            {
                get
                {
                    return this.webCategoryNmField;
                }
                set
                {
                    this.webCategoryNmField = value;
                }
            }

            /// <remarks/>
            public byte WebRating
            {
                get
                {
                    return this.webRatingField;
                }
                set
                {
                    this.webRatingField = value;
                }
            }

            /// <remarks/>
            public byte LimitAge
            {
                get
                {
                    return this.limitAgeField;
                }
                set
                {
                    this.limitAgeField = value;
                }
            }

            /// <remarks/>
            public byte SwPikadon
            {
                get
                {
                    return this.swPikadonField;
                }
                set
                {
                    this.swPikadonField = value;
                }
            }

            /// <remarks/>
            public byte TrailingItem
            {
                get
                {
                    return this.trailingItemField;
                }
                set
                {
                    this.trailingItemField = value;
                }
            }

            /// <remarks/>
            public byte KodSet
            {
                get
                {
                    return this.kodSetField;
                }
                set
                {
                    this.kodSetField = value;
                }
            }

            /// <remarks/>
            public byte SetId
            {
                get
                {
                    return this.setIdField;
                }
                set
                {
                    this.setIdField = value;
                }
            }

            /// <remarks/>
            public byte MinimumStock
            {
                get
                {
                    return this.minimumStockField;
                }
                set
                {
                    this.minimumStockField = value;
                }
            }

            /// <remarks/>
            public byte MaximumStock
            {
                get
                {
                    return this.maximumStockField;
                }
                set
                {
                    this.maximumStockField = value;
                }
            }

            /// <remarks/>
            public byte StiaAM
            {
                get
                {
                    return this.stiaAMField;
                }
                set
                {
                    this.stiaAMField = value;
                }
            }

            /// <remarks/>
            public object NosafKod_4
            {
                get
                {
                    return this.nosafKod_4Field;
                }
                set
                {
                    this.nosafKod_4Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_4
            {
                get
                {
                    return this.nosafNm_4Field;
                }
                set
                {
                    this.nosafNm_4Field = value;
                }
            }

            /// <remarks/>
            public object NosafKod_5
            {
                get
                {
                    return this.nosafKod_5Field;
                }
                set
                {
                    this.nosafKod_5Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_5
            {
                get
                {
                    return this.nosafNm_5Field;
                }
                set
                {
                    this.nosafNm_5Field = value;
                }
            }

            /// <remarks/>
            public object NosafKod_6
            {
                get
                {
                    return this.nosafKod_6Field;
                }
                set
                {
                    this.nosafKod_6Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_6
            {
                get
                {
                    return this.nosafNm_6Field;
                }
                set
                {
                    this.nosafNm_6Field = value;
                }
            }

            /// <remarks/>
            public object NosafKod_7
            {
                get
                {
                    return this.nosafKod_7Field;
                }
                set
                {
                    this.nosafKod_7Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_7
            {
                get
                {
                    return this.nosafNm_7Field;
                }
                set
                {
                    this.nosafNm_7Field = value;
                }
            }

            /// <remarks/>
            public object NosafKod_8
            {
                get
                {
                    return this.nosafKod_8Field;
                }
                set
                {
                    this.nosafKod_8Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_8
            {
                get
                {
                    return this.nosafNm_8Field;
                }
                set
                {
                    this.nosafNm_8Field = value;
                }
            }

            /// <remarks/>
            public object NosafKod_9
            {
                get
                {
                    return this.nosafKod_9Field;
                }
                set
                {
                    this.nosafKod_9Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_9
            {
                get
                {
                    return this.nosafNm_9Field;
                }
                set
                {
                    this.nosafNm_9Field = value;
                }
            }

            /// <remarks/>
            public object NosafKod_10
            {
                get
                {
                    return this.nosafKod_10Field;
                }
                set
                {
                    this.nosafKod_10Field = value;
                }
            }

            /// <remarks/>
            public object NosafNm_10
            {
                get
                {
                    return this.nosafNm_10Field;
                }
                set
                {
                    this.nosafNm_10Field = value;
                }
            }

            /// <remarks/>
            public byte SwAlcohol
            {
                get
                {
                    return this.swAlcoholField;
                }
                set
                {
                    this.swAlcoholField = value;
                }
            }

            /// <remarks/>
            public byte SwMustCmtInKupa
            {
                get
                {
                    return this.swMustCmtInKupaField;
                }
                set
                {
                    this.swMustCmtInKupaField = value;
                }
            }

            /// <remarks/>
            public string ManufacturingCountry
            {
                get
                {
                    return this.manufacturingCountryField;
                }
                set
                {
                    this.manufacturingCountryField = value;
                }
            }

            /// <remarks/>
            public byte PikadonCmt
            {
                get
                {
                    return this.pikadonCmtField;
                }
                set
                {
                    this.pikadonCmtField = value;
                }
            }

            /// <remarks/>
            public byte IsControlledItem
            {
                get
                {
                    return this.isControlledItemField;
                }
                set
                {
                    this.isControlledItemField = value;
                }
            }

            /// <remarks/>
            public byte MinQtyForSale
            {
                get
                {
                    return this.minQtyForSaleField;
                }
                set
                {
                    this.minQtyForSaleField = value;
                }
            }

            /// <remarks/>
            public byte MaxQtyForSale
            {
                get
                {
                    return this.maxQtyForSaleField;
                }
                set
                {
                    this.maxQtyForSaleField = value;
                }
            }

            /// <remarks/>
            public uint PrtC
            {
                get
                {
                    return this.prtCField;
                }
                set
                {
                    this.prtCField = value;
                }
            }

            /// <remarks/>
            public object ColorGrp
            {
                get
                {
                    return this.colorGrpField;
                }
                set
                {
                    this.colorGrpField = value;
                }
            }

            /// <remarks/>
            public byte Ref_Line
            {
                get
                {
                    return this.ref_LineField;
                }
                set
                {
                    this.ref_LineField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
        public partial class ArrayOfClsItemsClsItemsItemModel
        {

            private string nameField;

            private string idField;

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public string ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
        public partial class ArrayOfClsItemsClsItemsItemColor
        {

            private object nameField;

            private object idField;

            /// <remarks/>
            public object Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public object ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
        public partial class ArrayOfClsItemsClsItemsItemSize
        {

            private object nameField;

            private object idField;

            /// <remarks/>
            public object Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            public object ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }
        }


    }
}
