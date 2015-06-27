using System.Web.Http;
using Products_Core.Adapters.DataAccess;

namespace Product_API.Adapters.Controllers
{
    public class FeedController : ApiController
    {
        public FeedController(IProductsDAO productsDao)
        {
            
        }
    }
}
