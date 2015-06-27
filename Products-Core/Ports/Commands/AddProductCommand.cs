using System;
using paramore.brighter.commandprocessor;

namespace Products_Core.Ports.Commands
{
    public class AddProductCommand : Command
    {
        public AddProductCommand(string productName, string productDescription) : base(Guid.NewGuid())
        {
            ProductName = productName;
            ProductDescription = productDescription;
        }

        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }

    }
}
