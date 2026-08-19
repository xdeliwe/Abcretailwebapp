using Azure;
using Azure.Data.Tables;
namespace Abcetailwebapp.Models
{
    public class Product : ITableEntity
    {
        public string PartitionKey { get; set; }= "Product";
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

    }
}
