using Products_Core.Adapters.DataAccess;
using Products_Core.Ports.Resources;

namespace Products_Core.Ports.ViewModelRetrievers
{
    public class ProductModelRetriever
    {
       private readonly IProductsDAO _productsDao;
        private readonly string _hostName;

        public ProductModelRetriever(string hostName, IProductsDAO productsDao)
        {
            _hostName = hostName;
            _productsDao = productsDao;
        }

        public dynamic RetrieveProduct(int productId)
        {
            var product = _productsDao.FindById(productId);
            var productModel = new ProductModel(product, _hostName);
            return productModel;
        }    
    }
}
