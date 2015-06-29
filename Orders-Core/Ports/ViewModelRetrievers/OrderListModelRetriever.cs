using Orders_Core.Adapters.DataAccess;
using Orders_Core.Ports.Resources;

namespace Orders_Core.Ports.ViewModelRetrievers
{
   public class OrderListModelRetriever 
    {
        private readonly IOrdersDAO _ordersDao;
        private readonly string _hostName;

        public OrderListModelRetriever (string hostName, IOrdersDAO ordersDao)
        {
            _hostName = hostName;
            _ordersDao = ordersDao;
        }

        public dynamic RetrieveOrders()
        {
            var orders = _ordersDao.FindAll();
            var orderList = new OrderListModel(orders, _hostName);
            return orderList;
        }
    }
}
