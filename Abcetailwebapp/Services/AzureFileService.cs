using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
namespace Abcetailwebapp.Services
{
    public class AzureFileService
    {
        
        private readonly string _connectionString;
        
        public AzureFileService(AzureStorageService azureStorageService)
        {
            _connectionString = azureStorageService.GetConnectionString();
        }
        // Create a file share named "abc-files" if it doesn't exist and return the ShareClient
        public async Task<ShareClient> GetShareClientAsync()
        {
            var shareClient = new ShareClient(_connectionString,"abc-files");
            await shareClient.CreateIfNotExistsAsync();
            return shareClient;
        }
        // Upload a file to the file share
        public async Task UploadFileAsync(IFormFile file)
        {
            var shareClient = await GetShareClientAsync();
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(file.FileName);
            using var stream = file.OpenReadStream();
            
            await fileClient.DeleteIfExistsAsync();
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadAsync(stream);
        }
    }
}
