using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.policy.Attributes;
using Products_Core.Adapters.DataAccess;
using Products_Core.Model;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Events;

namespace Products_Core.Ports.Handlers
{
    public class AddProductCommandHandler: RequestHandler<AddProductCommand>
    {
        private readonly IProductsDAO _productsDao;
        private readonly IAmACommandProcessor _commandProcessor;

        public AddProductCommandHandler(IProductsDAO productsDao, IAmACommandProcessor commandProcessor,  ILog logger) : base(logger)
        {
            _productsDao = productsDao;
            _commandProcessor = commandProcessor;
        }

        [RequestLogging(step: 1, timing: HandlerTiming.Before)]
        [UsePolicy(CommandProcessor.RETRYPOLICY, step: 3)]
        public override AddProductCommand Handle(AddProductCommand addProductCommand)
        {
            Product insertedProduct;
            using (var scope = _productsDao.BeginTransaction())
            {
                insertedProduct = _productsDao.Add(
                    new Product(
                        productName: addProductCommand.ProductName,
                        productDescription: addProductCommand.ProductDescription,
                        productPrice: addProductCommand.ProductPrice)
                    );

                scope.Commit();

            }

            if (insertedProduct != null)
                _commandProcessor.Publish(new ProductAddedEvent(insertedProduct.ProductId, insertedProduct.ProductName, insertedProduct.ProductDescription, insertedProduct.ProductPrice));

            return base.Handle(addProductCommand);
        }
    }
}
