using System.Runtime.Serialization;

namespace Products_Core.Adapters.Atom
{
    [DataContract(Name = "product-entry-type", Namespace = "urn:paramore:samples:cakeshop")]
    public enum ProductEntryType
    {
        [EnumMember]
        Created,
        [EnumMember]
        Updated,
        [EnumMember]
        Deleted
    }

    [DataContract(Name="product-entry", Namespace = "urn:paramore:samples:cakeshop")]
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
        [DataMember (Name = "type")]
        public ProductEntryType Type { get; set; }
        [DataMember (Name = "description")]
        public string ProductDescription { get; set; }

        [DataMember (Name = "id")]
        public int ProductId { get; set; }
        [DataMember (Name = "name")]
        public string ProductName { get; set; }

    }
}
