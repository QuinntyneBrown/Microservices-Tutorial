using System;
using paramore.brighter.commandprocessor;

namespace Products_Core.Ports.Commands
{
    public class RemoveProductCommand : Command
    {
        public RemoveProductCommand(int productId) : base(Guid.NewGuid())
        {
            ProductId = productId;
        }

        public int ProductId { get; private set; }
    }
}
