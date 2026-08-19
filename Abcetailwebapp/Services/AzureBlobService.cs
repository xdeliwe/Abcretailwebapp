using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace Abcetailwebapp.Services
{
    public class AzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlobService(AzureStorageService azureStorageService)
        {
            _blobServiceClient = azureStorageService.GetBlobServiceClient();
        }
        // Create a blob container named "product-images" if it doesn't exist and return the BlobContainerClient
        public async Task<BlobContainerClient>GetBlobContainerClientAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("product-images");
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            return containerClient;
        }
        // Upload an image to the blob container and return the URL of the uploaded image
        public async Task<string> UploadImageAsync(IFormFile image,string productId)
        {
            var containerClient = await GetBlobContainerClientAsync();
            var extension = Path.GetExtension(image.FileName);
            var blobName = $"{productId}{extension}";
            var blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = image.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
            }
            return blobClient.Uri.ToString();
            
             
        }
        
    }
}
