using System;
using paramore.brighter.commandprocessor;

namespace Products_Core.Ports.Events
{
    public class ProductAddedEvent : Event
    {
        public string ProductDescription { get; set; }
        public int ProductId { get; set; }
        public double ProductPrice { get; set; }
        public string ProductName { get; set; }

        public ProductAddedEvent(int productId, string productName, string productDescription, double productPrice) : base(Guid.NewGuid())
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
        }
    }
}
