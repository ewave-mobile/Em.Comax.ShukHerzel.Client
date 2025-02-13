using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    public class SalesPriceWrapperDto
    {
        [XmlElement(ElementName = "ClsItemPrices")]
        public ItemPriceDto ClsItemPrices { get; set; }
    }
}
