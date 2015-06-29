using Products_Core.Adapters.DataAccess;
using Products_Core.Ports.Resources;

namespace Products_Core.Ports.ViewModelRetrievers
{
    public class ProductListModelRetriever
    {
        private readonly IProductsDAO _productsDao;
        private readonly string _hostName;

        public ProductListModelRetriever(string hostName, IProductsDAO productsDao)
        {
            _hostName = hostName;
            _productsDao = productsDao;
        }

        public dynamic RetrieveProducts()
        {
            var products = _productsDao.FindAll();
            var productListModel = new ProductListModel(products, _hostName);
            return productListModel;
        }
    }
}
