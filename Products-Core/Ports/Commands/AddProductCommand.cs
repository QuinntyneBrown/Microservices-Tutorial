using System;
using paramore.brighter.commandprocessor;

namespace Products_Core.Ports.Commands
{
    public class AddProductCommand : Command
    {
        public AddProductCommand(string productName, string productDescription, double productPrice) : base(Guid.NewGuid())
        {
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
        }

        public string ProductDescription { get; private set; }
        public string ProductName { get; private set; }
        public double ProductPrice { get; set; }
        public int ProductId { get; set; }
    }
}
