using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    public class ItemPriceDto
    {
        [XmlElement(ElementName = "PriceListID")]
        public int PriceListID { get; set; }

        [XmlElement(ElementName = "Currency")]
        public string Currency { get; set; }

        [XmlElement(ElementName = "IsIncludeVat")]
        public bool IsIncludeVat { get; set; }

        [XmlElement(ElementName = "Price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName = "NetPrice")]
        public decimal NetPrice { get; set; }

        [XmlElement(ElementName = "ShekelPrice")]
        public decimal ShekelPrice { get; set; }

        [XmlElement(ElementName = "ShekelNetPrice")]
        public decimal ShekelNetPrice { get; set; }

        [XmlElement(ElementName = "SalePrice")]
        public decimal SalePrice { get; set; }

        // If "0" indicates no date or special meaning, you might store it as string
        // Otherwise, you could use DateTime? with a custom converter if needed.
        [XmlElement(ElementName = "OperationEndDate")]
        public string OperationEndDate { get; set; }
    }
}
