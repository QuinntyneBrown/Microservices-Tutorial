using System.Web.Http;
using Orders_Core;
using Orders_Core.Adapters.DataAccess;
using Orders_Core.Ports.Resources;
using Orders_Core.Ports.ViewModelRetrievers;

namespace Orders_API.Adapters.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly IOrdersDAO _ordersDao;

        public OrdersController(OrdersDAO ordersDao)
        {
            _ordersDao = ordersDao;
        }

        [HttpGet]
        public OrderListModel Get()
        {
            var orderRetriever = new OrderListModelRetriever(Globals.HostName, _ordersDao);
            return orderRetriever.RetrieveOrders();

        }
    }
}
