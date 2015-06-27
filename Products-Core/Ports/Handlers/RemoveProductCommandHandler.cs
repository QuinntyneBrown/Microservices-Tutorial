using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.DataAccess;
using Products_Core.Model;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Events;

namespace Products_Core.Ports.Handlers
{
    public class RemoveProductCommandHandler : RequestHandler<RemoveProductCommand>
    {
        private readonly IProductsDAO _productsDao;
        private readonly IAmACommandProcessor _commandProcessor;

        public RemoveProductCommandHandler(IProductsDAO productsDao, IAmACommandProcessor commandProcessor, ILog logger) : base(logger)
        {
            _productsDao = productsDao;
            _commandProcessor = commandProcessor;
        }

        public override RemoveProductCommand Handle(RemoveProductCommand removeProductCommand)
        {
            Product product;
            using (var scope = _productsDao.BeginTransaction())
            {
                product = _productsDao.FindById(removeProductCommand.ProductId);
                _productsDao.Delete(removeProductCommand.ProductId);
            }

            if (product != null)
                _commandProcessor.Publish(new ProductRemovedEvent(product.ProductId, product.ProductName, product.ProductDescription, product.ProductPrice));

            return base.Handle(removeProductCommand);
        }
    }
}
