using Azure.Storage.Queues;
using System.Text.Json;
namespace Abcetailwebapp.Services
{
    public class AzureQueueService
    {
        private readonly QueueClient _queueClient;

        public AzureQueueService(AzureStorageService azureStorageService)
        {
            _queueClient = azureStorageService.GetQueueClient("order-queue");
        }
        // Send a message to the queue
        public async Task SendMessageAsync(object message)
        {
            await _queueClient.CreateIfNotExistsAsync();
            string messageJson = JsonSerializer.Serialize(message);
            await _queueClient.SendMessageAsync(messageJson);
        }
        // Retrieve messages from the queue without deleting them
        public async Task<List<string>> GetMessagesAsync()
        {
            await _queueClient.CreateIfNotExistsAsync();

            var messages = new List<string>();
            var response = await _queueClient.ReceiveMessagesAsync(maxMessages: 32);
            foreach (var message in response.Value)
            {
                messages.Add(message.MessageText);
               /* await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);*/
            }
            return messages;
        }
    }
}
