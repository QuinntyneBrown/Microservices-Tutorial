using System.Runtime.Serialization;
using System.Xml.Serialization;
using Orders_Core.Model;

namespace Orders_Core.Ports.Resources
{
    [DataContract, XmlRoot]
    public class OrderListItemModel
    {
        [DataMember(Name = "self"), XmlElement(ElementName = "self")]
        public Link Self{ get; private set; }
        [DataMember(Name = "customerName"), XmlElement(ElementName = "customerName")]
        public string CustomerName { get; private set; }
        [DataMember(Name = "description"), XmlElement(ElementName = "description")]
        public string Description { get; private set; }
        [DataMember(Name = "completionDate"), XmlElement(ElementName = "completionDate")]
        public string CompletionDate { get; private set; }
        [DataMember(Name = "dueDate"), XmlElement(ElementName = "dueDate")]
        public string DueDate { get; private set; }
        [DataMember(Name = "href"), XmlElement(ElementName = "href")]
        public string HRef { get; private set; }
        [DataMember(Name = "isComplete"), XmlElement(ElementName = "isComplete")]
        public bool IsComplete { get; set; }
        [DataMember(Name = "id"), XmlElement(ElementName = "id")]
        public int Id { get; set; }

        public static OrderListItemModel Create(Order order, string hostName)
        {
            var hRef = string.Format("http://{0}/{1}/{2}", hostName, "orders", order.Id);
            return new OrderListItemModel
            {
                HRef = hRef,
                Self = new Link { Rel = "item", HRef = hRef },
                CustomerName = order.CustomerName,
                Description = order.OrderDescription,
                IsComplete = order.CompletionDate.HasValue,
                CompletionDate = order.CompletionDate.ToDisplayString(),
                DueDate = order.DueDate.ToDisplayString(),
                Id = order.Id
            };
        }

    }
}