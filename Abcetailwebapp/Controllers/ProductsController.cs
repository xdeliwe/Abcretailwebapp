using Microsoft.AspNetCore.Mvc;
using Abcetailwebapp.Services;
using Abcetailwebapp.Models;
namespace Abcetailwebapp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AzureTableService _azureTableService;
        private readonly AzureBlobService _azureBlobService;
        public ProductsController(AzureTableService azureTableService,AzureBlobService azureBlobService)
        {
            _azureTableService = azureTableService;
            _azureBlobService = azureBlobService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _azureTableService.GetAllProductsAsync();
            return View(products);
        }
        [HttpGet]
        // The Edit action method retrieves a product by its ID for editing. It first checks if the provided ID is null or empty, returning a NotFound result if it is. It then attempts to retrieve the product from Azure Table Storage using the provided ID. If the product is not found, it returns a NotFound result. If the product is found, it returns the view with the product data for editing.
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var product = await _azureTableService.GetProductAsync(id);
            if (product == null)
            { 
                return NotFound();
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // The Edit action method handles the form submission for editing a product. It takes the product ID, the updated product data, and an optional image file as parameters. The method first checks if the provided ID matches the product's ID. If not, it returns a NotFound result. It then retrieves the existing product from the Azure Table Storage.
        // If the product doesn't exist, it returns NotFound. If the model state is invalid, it returns the view with the existing product data. If an image file is provided, it uploads the image to Azure Blob Storage and updates the product's ImageUrl. Finally, it updates the product in Azure Table Storage and redirects to the Index action with a success message.
        public async Task<IActionResult> Edit(string id, Product product, IFormFile? image)
        {
            Console.WriteLine($"IMAGE: {image?.FileName}, SIZE: {image?.Length}");
            if (id != product.ProductId)
            {
                return NotFound();
            }

            var existingProduct = await _azureTableService.GetProductAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                product.ImageUrl = existingProduct.ImageUrl;
                return View(product);
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;

            if (image != null && image.Length > 0)
            {
                existingProduct.ImageUrl =
                    await _azureBlobService.UploadImageAsync(
                        image,
                        existingProduct.ProductId);
            }

            await _azureTableService.UpdateProductAsync(existingProduct);

            TempData["SuccessMessage"] = "Product updated successfully!";

            return RedirectToAction(nameof(Index));
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(string id, Product product, IFormFile? image)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            product.PartitionKey = "Product";
            product.RowKey = product.ProductId;
            if (image != null && image.Length > 0)
            {
                product.ImageUrl = await _azureBlobService.UploadImageAsync(image,product.ProductId);

            }
            await _azureTableService.UpdateProductAsync(product);
            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }*/
      
        // GET: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        // The Delete action method handles the deletion of a product. It takes the product ID as a parameter, retrieves the product from Azure Table Storage, and if found, deletes it. After deletion, it redirects to the Index action with a success message.
        public async Task<IActionResult> Create(Product product, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            product.PartitionKey = "Product";
            product.RowKey = product.ProductId;
            
            if (image != null && image.Length > 0)
            {
                product.ImageUrl = await _azureBlobService.UploadImageAsync(image, product.ProductId);
            }
            await _azureTableService.AddProductAsync(product);
            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}