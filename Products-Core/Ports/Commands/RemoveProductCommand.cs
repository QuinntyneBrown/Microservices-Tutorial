using System;
using paramore.brighter.commandprocessor;

namespace Products_Core.Ports.Commands
{
    public class RemoveProductCommand : Command
    {
        public RemoveProductCommand(Guid productId) : base(Guid.NewGuid())
        {
            ProductId = productId;
        }

        public Guid ProductId { get; private set; }
    }
}
