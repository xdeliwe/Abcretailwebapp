using Abcetailwebapp.Models;
using Azure.Data.Tables;

namespace Abcetailwebapp.Services
{
    public class AzureTableService
    {
        private readonly TableServiceClient tableServiceClient;

        public AzureTableService(AzureStorageService azureStorageService)
        {
            tableServiceClient = azureStorageService.GetTableServiceClient();
        }
        public async Task CreateTablesAsync()
        {
            await tableServiceClient.CreateTableIfNotExistsAsync("Customers");
            await tableServiceClient.CreateTableIfNotExistsAsync("Products");
        }
        // Connect to the Customers table and return the TableClient
        public TableClient GetCustomersTable()
        {
            return tableServiceClient.GetTableClient("Customers");
        }
        // Add a new customer to the Customers table
        public async Task AddCustomerAsync(Customer customer)
        {
            var tableClient = GetCustomersTable();
            await tableClient.AddEntityAsync(customer);
        }
        // Get all customers from the Customers table
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var tableClient = GetCustomersTable();
            var customers = new List<Customer>();
            await foreach (var customer in tableClient.QueryAsync<Customer>())
            {
                customers.Add(customer);
            }
            return customers;
        }
        public TableClient GetProductsTable()
        {
            return tableServiceClient.GetTableClient("Products");
        }
        // Add a new product to the Products table
        public async Task AddProductAsync(Product product)
        {
            var tableClient = GetProductsTable();
            await tableClient.AddEntityAsync(product);
        }
        // Get all products from the Products table
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var tableClient = GetProductsTable();
            var products = new List<Product>();
            await foreach (var product in tableClient.QueryAsync<Product>())
            {
                products.Add(product);
            }
            return products;
        }
        // Upload an image to Azure Blob Storage and return the URL
        internal async Task<string> UploadImageToBlobAsync(IFormFile image)
        {
            throw new NotImplementedException();
        }
        // Get a product by its ID from the Products table
        public async Task<Product?>GetProductAsync(string productId)
        {
            var tablClient = GetProductsTable();
            try
            {
                var response = await tablClient.GetEntityAsync<Product>("Product", productId);
                return response.Value;
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            { 
                return null;
            }
        }
        // Update a product in the Products table
        public async Task UpdateProductAsync(Product product)
        { 
            var tableClient = GetProductsTable();
            await tableClient.UpdateEntityAsync(product,product.ETag,TableUpdateMode.Replace);
        }

    }
}
