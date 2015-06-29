using System.Runtime.Serialization;
using System.Xml.Serialization;
using Products_Core.Model;

namespace Products_Core.Ports.Resources
{
    [DataContract, XmlRoot]
    public class ProductModel
    {

        public ProductModel() { }

        public ProductModel(Product product, string hostName)
        {
            ProductName = product.ProductName;
            ProductDescription = product.ProductDescription;
            ProductPrice = product.ProductPrice;
            Id = product.Id;
            Self = new Link {Rel = "item", HRef = string.Format("http://{0}/{1}/{2}", hostName, "products", product.Id)};
        }

        [DataMember(Name = "id"), XmlElement(ElementName = "id")]
        public int Id { get; set; }
        [DataMember(Name = "productDescription"), XmlElement(ElementName = "productDescription")]
        public string ProductDescription { get; set; }
        [DataMember(Name = "productName"), XmlElement(ElementName = "productName")]
        public string ProductName { get; set; }
        [DataMember(Name = "productPrice"), XmlElement(ElementName = "productPrice")]
        public double ProductPrice { get; set; }
        [DataMember(Name = "self"), XmlElement(ElementName = "self")]
        public Link Self { get; set; }
    }
}
