using System;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.Atom;
using Products_Core.Model;
using Products_Core.Ports.Events;

namespace Products_Core.Ports.Handlers
{
    public class ProductChangedEventHandler : RequestHandler<ProductChangedEvent>
    {
        private readonly IObserver<ProductEntry> _observer;

        public ProductChangedEventHandler(IObserver<ProductEntry> observer, ILog logger) : base(logger)
        {
            _observer = observer;
        }

        public override ProductChangedEvent Handle(ProductChangedEvent productChangedEvent)
        {
            _observer.OnNext(new ProductEntry(
                type: ProductEntryType.Updated,
                productId: productChangedEvent.ProductId,
                productDescription: productChangedEvent.ProductDescription, 
                productName: productChangedEvent.ProductName, 
                productPrice: productChangedEvent.ProductPrice));

            return base.Handle(productChangedEvent);
        }
    }
}
