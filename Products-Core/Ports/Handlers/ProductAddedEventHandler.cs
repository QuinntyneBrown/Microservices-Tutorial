using System;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.Atom;
using Products_Core.Ports.Events;

namespace Products_Core.Ports.Handlers
{
    public class ProductAddedEventHandler : RequestHandler<ProductAddedEvent>
    {
        private readonly IObserver<ProductEntry> _observer;

        public ProductAddedEventHandler(IObserver<ProductEntry> observer, ILog logger) : base(logger)
        {
            _observer = observer;
        }

        public override ProductAddedEvent Handle(ProductAddedEvent productAddedEvent)
        {
            _observer.OnNext(new ProductEntry(
                type: ProductEntryType.Created,
                productId: productAddedEvent.ProductId,
                productDescription: productAddedEvent.ProductDescription, 
                productName: productAddedEvent.ProductName, 
                productPrice: productAddedEvent.ProductPrice));

            return base.Handle(productAddedEvent);
        }
    }
}
