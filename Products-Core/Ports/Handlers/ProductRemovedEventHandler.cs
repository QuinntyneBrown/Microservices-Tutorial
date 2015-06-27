using System;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.Atom;
using Products_Core.Ports.Events;

namespace Products_Core.Ports.Handlers
{
    public class ProductRemovedEventHandler : RequestHandler<ProductRemovedEvent>
    {
        private readonly IObserver<ProductEntry> _observer;

        public ProductRemovedEventHandler(IObserver<ProductEntry> observer, ILog logger) : base(logger)
        {
            _observer = observer;
        }

        public override ProductRemovedEvent Handle(ProductRemovedEvent productRemovedEvent)
        {
            _observer.OnNext(new ProductEntry(
                type: ProductEntryType.Deleted,
                productId: productRemovedEvent.ProductId,
                productDescription: productRemovedEvent.ProductDescription, 
                productName: productRemovedEvent.ProductName, 
                productPrice: productRemovedEvent.ProductPrice));

            return base.Handle(productRemovedEvent);
        }
    }
}
