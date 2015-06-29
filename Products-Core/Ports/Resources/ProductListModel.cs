using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Products_Core.Model;

namespace Products_Core.Ports.Resources
{
      [DataContract, XmlRoot]
    public class ProductListModel
    {
        private Link _self;
        private IEnumerable<ProductModel> _items;

        public ProductListModel(IEnumerable<Product> products, string hostName)
        {
            _self = Link.Create(this, hostName);
            _items = products.Select(product => new ProductModel(product, hostName));
        }

        [DataMember(Name = "self"), XmlElement(ElementName = "self")]
        public Link Self
        {
            get { return _self; }
            set { _self = value; }
        }

        [DataMember(Name = "items"), XmlElement(ElementName = "items")]
        public IEnumerable<ProductModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}
