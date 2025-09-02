using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{

    public class PromotionDto
    {
        public D[] d { get; set; }
    }

    public class D
    {
        public string? __type { get; set; }
        public string? Kod { get; set; }
        public string? Nm { get; set; }
        public string? RemarkForPrint { get; set; }
        public string? Nature { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? SwActive { get; set; }
        public string? SwSunday { get; set; }
        public string? ActiveFor_Hour_Sunday { get; set; }
        public string? ActiveUpTo_Hour_Sunday { get; set; }
        public string? SwMonday { get; set; }
        public string? ActiveFor_Hour_Monday { get; set; }
        public string? ActiveUpTo_Hour_Monday { get; set; }
        public string? SwTuesday { get; set; }
        public string? ActiveFor_Hour_Tuesday { get; set; }
        public string? ActiveUpTo_Hour_Tuesday { get; set; }
        public string? SwWednesday { get; set; }
        public string ActiveFor_Hour_Wednesday { get; set; }
        public string? ActiveUpTo_Hour_Wednesday { get; set; }
        public string? SwThursday { get; set; }
        public string? ActiveFor_Hour_Thursday { get; set; }
        public string? ActiveUpTo_Hour_Thursday { get; set; }
        public string? SwFriday { get; set; }
        public string? ActiveFor_Hour_Friday { get; set; }
        public string? ActiveUpTo_Hour_Friday { get; set; }
        public string? SwSaturday { get; set; }
        public string? ActiveFor_Hour_Saturday { get; set; }
        public string? ActiveUpTo_Hour_Saturday { get; set; }
        public string? SwKupa { get; set; }
        public string? RealizationPercent { get; set; }
        public string? SwAllBranches { get; set; }
        public string? SwAllCustomers { get; set; }
        public string? SwAllItems { get; set; }
        public string? SwPrintNm { get; set; }
        public string? SwSignageOnly { get; set; }
        public string? SwCasing { get; set; }
        public string? SwIncludeRelatedCompStores { get; set; }
        public string? PromotionType { get; set; }
        public string? SupplierName { get; set; }
        public string? Quantity { get; set; }
        public string? MinQty { get; set; }
        public string? MaxQty { get; set; }
        public string? Total { get; set; }
        public string? GetGiftItem { get; set; }
        public string? GetRemark { get; set; }
        public string? GetCmt { get; set; }
        public string? SwIncludeNetoItem { get; set; }
        public string? GetTotal { get; set; }
        public string? GetDiscountPrecent { get; set; }
        public string? GetDiscountTotal { get; set; }
        public string? TotalForActivate { get; set; }
        public string? SwSameDiffItems { get; set; }
        public string? WithoutPrintContent { get; set; }
        public string? Rating { get; set; }
        public string? NoAdditionalDiscounts { get; set; }
        public string? WithoutPresentList { get; set; }
        public string? WithoutPrintingData { get; set; }
        public string? Classified { get; set; }
        public string? MaxInDoc { get; set; }
        public string? SwCalcEnd { get; set; }
        public string? SwCheck_ForTotalNeto { get; set; }
        public string? SwCalcDis { get; set; }
        public string? SwMustPay_ClubCredit { get; set; }
        public string? SpurMessage { get; set; }
        public string? SpurTotal { get; set; }
        public string? SpurQty { get; set; }
        public string? DoubleDeals { get; set; }
        public string? WithoutMarkOnWeb { get; set; }
        public string? SwSupplierCharge { get; set; }
        public string? SupplierForCharge { get; set; }
        public string? PriceListForCharge { get; set; }
        public string? SwChargeType { get; set; }
        public string? TotalDiscountCharge { get; set; }
        public string? SwOperative { get; set; }
        public string? SwNoSplit { get; set; }
        public string? MustAdditionalPromotions { get; set; }
        public string? TextForWeb { get; set; }
        public string? TextToPrint { get; set; }
        public string? TextToPrint_Unicode { get; set; }
        public string? ApprovedSignage { get; set; }
        public string? Tag1 { get; set; }
        public string? Tag2 { get; set; }
        public string? SelfFinancingReward { get; set; }
        public string? PromoForRealization { get; set; }
        public string? CostOfRealizingGift { get; set; }
        public string? SelectPromo_ToMultiply { get; set; }
        public string SelectPromo_ToNotMultiply { get; set; }
        public Store[] Stores { get; set; }
        public Customergrp[] CustomerGrp { get; set; }
        public Item[] Items { get; set; }
        public object[] Suppliers { get; set; }
        public object[] ItemsGrp { get; set; }
        public object[] ItemsSubGrp { get; set; }
        public object[] ItemsDep { get; set; }
        public object[] ItemsModel { get; set; }
        public object[] ItemsVarious { get; set; }
        public object[] ItemsAttribute1 { get; set; }
        public object[] ItemsAttribute2 { get; set; }
        public object[] ItemsAttribute3 { get; set; }
        public object GetItems { get; set; }
        public object GetSuppliers { get; set; }
        public object GetItemsGrp { get; set; }
        public object GetItemsSubGrp { get; set; }
        public object GetItemsDep { get; set; }
        public object GetItemsModel { get; set; }
        public object GetItemsAttribute1 { get; set; }
        public object GetItemsAttribute2 { get; set; }
        public object GetItemsAttribute3 { get; set; }
        public object ErrorMessage { get; set; }
        public string? ClassifiedNm { get; set; }
        public string? CounterGet_InSale { get; set; }
        public object SameGetAndBuy { get; set; }
        public string? SwNotForShelfSignage { get; set; }
    }

    public class Store
    {
        public string? Kod { get; set; }
        public string? Amount { get; set; }
        public string? ExerciseCostUnit { get; set; }
    }

    public class Customergrp
    {
        public string? C { get; set; }
        public string? Kod { get; set; }
        public string? SwNotActive { get; set; }
    }

    public class Item
    {
        public string? PrintImage { get; set; }
        public string? ExerciseCostUnit { get; set; }
        public string? BasketNum { get; set; }
        public string? C { get; set; }
        public string? Kod { get; set; }
        public string? SwNotActive { get; set; }
    }

}
