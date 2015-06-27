namespace Products_Core.Adapters.Atom
{
    public enum ProductEntryType
    {
        Created,
        Updated,
        Deleted
    }

    public class ProductEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ProductEntry(ProductEntryType type, int productId, string productDescription, string productName, double productPrice)
        {
            Type = type;
            ProductDescription = productDescription;
            ProductId = productId;
            ProductName = productName;
        }

        public ProductEntryType Type { get; set; }
        public string ProductDescription { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

    }
}
