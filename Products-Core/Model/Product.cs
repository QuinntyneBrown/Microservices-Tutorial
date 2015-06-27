using System;

namespace Products_Core.Model
{
    public class Product
    {

        public Product() { /*Required by Simple.Data*/}

        public Product(string productName, string productDescription, double productPrice)
        {
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
        }

        public string ProductDescription { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
    }
}
