using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Products_Core.Model;
using Simple.Data;

namespace Products_Core.Adapters.DataAccess
{
    class ProductDAO : IProductsDAO
    {
        private readonly dynamic _db;

        public ProductDAO()
        {
            if (System.Web.HttpContext.Current != null)
            {
                var databasePath = System.Web.HttpContext.Current.Server.MapPath("~\\App_Data\\Products.sdf");
                _db = Database.Opener.OpenFile(databasePath);
            }
            else
            {
                var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)), "App_Data\\Products.sdf");

                _db = Database.OpenFile(file);
            }
        }

        public dynamic BeginTransaction()
        {
            return _db.BeginTransaction();
        }

        public Product Add(Product newProduct)
        {
            return _db.Products.Insert(newProduct);
        }

        public IEnumerable<Product> FindAll()
        {
            return _db.Products.All().ToList<Product>();
        }

        public Product FindById(int id)
        {
            return _db.Orders.FindById(id);
        }

        public void Update(Product product)
        {
            _db.Products.UpdateById(product);
        }

        public void Clear()
        {
            _db.Products.DeleteAll();
        }
    }
}
