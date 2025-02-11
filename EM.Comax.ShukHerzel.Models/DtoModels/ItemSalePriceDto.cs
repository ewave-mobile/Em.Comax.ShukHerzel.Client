using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EM.Comax.ShukHerzel.Models.DtoModels
{
    public class ItemSalePriceDto
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        // Depending on the data you receive, you might choose string or int.
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }

        [XmlElement(ElementName = "Barcode")]
        public string Barcode { get; set; }

        [XmlElement(ElementName = "AlternateID")]
        public string AlternateID { get; set; }

        [XmlElement(ElementName = "SalesPrice")]
        public ItemPriceDto SalesPrice { get; set; }
    }
}
