using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    [XmlRoot("Get_AllItemsPricesBySearchResult", Namespace = "http://ws.comax.co.il/Comax_WebServices/")]
    public class GetAllItemsPricesBySearchResultDto
    {
        [XmlElement(ElementName = "ClsItemsSalePrices")]
        public List<ItemSalePriceDto> Items { get; set; }
    }
}
