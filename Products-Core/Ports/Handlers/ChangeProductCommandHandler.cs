using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.DataAccess;
using Products_Core.Model;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Events;

namespace Products_Core.Ports.Handlers
{
    public class ChangeProductCommandHandler: RequestHandler<ChangeProductCommand>
    {
        private readonly IProductsDAO _productsDao;
        private readonly IAmACommandProcessor _commandProcessor;

        public ChangeProductCommandHandler(IProductsDAO productsDao, IAmACommandProcessor commandProcessor, ILog logger) : base(logger)
        {
            _productsDao = productsDao;
            _commandProcessor = commandProcessor;
        }

        public override ChangeProductCommand Handle(ChangeProductCommand changeProductCommand)
        {

            Product product;
            using (var scope = _productsDao.BeginTransaction())
            {
                product = _productsDao.FindById(changeProductCommand.ProductId);
                product.ProductName = changeProductCommand.ProductName;
                product.ProductDescription = changeProductCommand.ProductDescription;
                product.ProductPrice = changeProductCommand.Price;

                _productsDao.Update(product);
                scope.Commit();
            }

            if (product != null)
                _commandProcessor.Publish(new ProductChangedEvent(product.ProductId, product.ProductName, product.ProductDescription, product.ProductPrice));

            return base.Handle(changeProductCommand);
        }
    }
}
