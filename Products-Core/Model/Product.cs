using System;

namespace Products_Core.Model
{
    public class Product
    {

        public Product() { /*Required by Simple.Data*/}

        public Product(string productName, string productDescription)
        {
            ProductName = productName;
            ProductDescription = productDescription;
        }

        public string ProductDescription { get; set; }
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
