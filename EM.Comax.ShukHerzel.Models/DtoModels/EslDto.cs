using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    public class EslDto
    {
        public string brand { get; set; } // Maps to Manufacturer
        public string id { get; set; } // Maps to Barcode
        public string name { get; set; } // Maps to ProductDesc
        public decimal? price { get; set; } // Maps to Price
       // public string size { get; set; } // Maps to Capacity
        public Custom custom { get; set; }
    }
    public class Custom
    {
        public string ManufacturingCountry { get; set; }
        public string Content { get; set; }
        public string ContentUnit { get; set; }
        public string ContentMeasure { get; set; }
        public string Quantity { get; set; }
        public string Total { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Kod { get; set; }
        public string SwAllCustomers { get; set; }
        public string TextForWeb { get; set; }
        public string SwWeighable { get; set; }
        public string StoreId { get; set; }
        public string AllBarcodes { get; set; }
        public string Size { get; set; }
        public string IsPromotion { get; set; }
        public string SwPikadon { get; set; }
        public string TrailingItem { get; set; }
        public string TotalForActivate { get; set; }
        public string PromotionQuantity { get; set; }
        public string GetDiscountTotal { get; set; }
        public string GetCmt { get; set; }
        public string GetDiscountPrecent { get; set; }
        public string GetTotal { get; set; }
        public string PromotionMinQty { get; set; }
        public string PromotionMaxQty { get; set; }


    }
}
