using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Orders_Core.Model;

namespace Orders_Core.Ports.Resources
{
     [DataContract, XmlRoot]
    public class OrderListModel
    {
        private Link _self;
        private IEnumerable<OrderListItemModel> _items;

        public OrderListModel(IEnumerable<Order> orders, string hostName)
        {
            _self = Link.Create(this, hostName);
            _items = orders.Select(task => OrderListItemModel.Create(task, hostName));
        }

        [DataMember(Name = "self"), XmlElement(ElementName = "self")]
        public Link Self
        {
            get { return _self; }
            set { _self = value; }
        }

        [DataMember(Name = "items"), XmlElement(ElementName = "items")]
        public IEnumerable<OrderListItemModel> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }

    public static class Extensions
    {
        public static string ToDisplayString(this DateTime? dateTimeToFormat)
        {
            if (!dateTimeToFormat.HasValue) return "";
            return dateTimeToFormat.Value.ToString("dd-MMM-yyyy HH:mm:ss");
        }
    }
}
