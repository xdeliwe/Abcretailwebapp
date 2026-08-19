using Abcetailwebapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Abcetailwebapp.Controllers
{
    public class QueueController : Controller
    {
        private readonly AzureQueueService _azureQueueService;

        public QueueController(AzureQueueService azureQueueService)
        {
            _azureQueueService = azureQueueService;
        }
        // GET: Queue/Index
        public async Task<IActionResult> Index()
        {
            var messages = await _azureQueueService.GetMessagesAsync();
            return View(messages);
        }
        [HttpPost]
        // POST: Queue/Send
        public async Task<IActionResult> Send(string productId,string customerId,int quantity)
        {
            var transaction = new { ProductId = productId, CustomerId = customerId, Quantity = quantity, TransactionType = "Order", Date = DateTime.UtcNow };
            await _azureQueueService.SendMessageAsync(transaction);
            return RedirectToAction("Index");
        }
    }
}
