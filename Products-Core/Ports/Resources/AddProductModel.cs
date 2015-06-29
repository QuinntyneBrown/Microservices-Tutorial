using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Products_Core.Ports.Resources
{
    public class AddProductModel
    {
        [DataMember(Name = "productDescription"), XmlElement(ElementName = "productDescription")]
        public string ProductDescription { get; set; }
        [DataMember(Name = "productName"), XmlElement(ElementName = "productName")]
        public string ProductName { get; set; }
        [DataMember(Name = "productPrice"), XmlElement(ElementName = "productPrice")]
        public double ProductPrice { get; set; }
    }
}
