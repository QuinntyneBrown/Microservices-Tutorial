using System;
using paramore.brighter.commandprocessor;

namespace Products_Core.Ports.Commands
{
    public class ChangeProductCommand : Command
    {
        public ChangeProductCommand(int productId, string productName, string productDescription, double price) : base(Guid.NewGuid())
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            Price = price;
        }

        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public double Price { get; private set; }
    }
}
