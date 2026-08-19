using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;

namespace Abcetailwebapp.Services
{
    public class AzureStorageService
    {
        // Get a QueueClient for a specific queue
        public QueueClient GetQueueClient(string queueName)
        {
            return new QueueClient(_connectionString, queueName);
        }
        private readonly string _connectionString;
        // Constructor to initialize the AzureStorageService with the connection string from configuration
        public AzureStorageService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureStorage:ConnectionString"] ?? throw new InvalidOperationException("Azure Storage connection string is not configured.");
        }
        // Get a TableServiceClient for interacting with Azure Table Storage    
        public TableServiceClient GetTableServiceClient()
        {
            return new TableServiceClient(_connectionString);
        }
        
        public BlobServiceClient GetBlobServiceClient()
        {
            return new BlobServiceClient(_connectionString);
        }
        // Get the connection string for Azure Storage
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}

