using Microsoft.AspNetCore.Mvc;
using Abcetailwebapp.Services;
using Abcetailwebapp.Models;
namespace Abcetailwebapp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AzureTableService _azureTableService;
      
        public CustomersController(AzureTableService azureTableService)
        {
            _azureTableService = azureTableService;
        }

        // GET the customers from the Azure Table Storage and return the view
        public async Task<IActionResult> Index()
        {
            var customers = await _azureTableService.GetAllCustomersAsync();
            return View(customers);
        }
        // GET the create customer view
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // POST the new customer to the Azure Table Storage and redirect to the index view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)

            {
                return View(customer);
            }
            customer.PartitionKey = "Customers";
            customer.RowKey = customer.CustomerId;

            await _azureTableService.AddCustomerAsync(customer);
            TempData["SuccessMessage"] = "Customer created successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
