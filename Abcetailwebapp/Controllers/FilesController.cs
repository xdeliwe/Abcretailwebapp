using Abcetailwebapp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Abcetailwebapp.Controllers
{
    public class FilesController : Controller
    {
        private readonly AzureFileService _azureFileService;

        public FilesController(AzureFileService azureFileService)
        {
            _azureFileService = azureFileService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        // The Upload action method handles the file upload process. It takes an IFormFile parameter representing the uploaded file.
        // The method first checks if the file is not null and has a length greater than zero. If the file is valid, it calls the UploadFileAsync method of the AzureFileService to upload the file to Azure File Storage. Upon successful upload, it sets a success message in TempData. If no file is received, it sets an appropriate message in TempData. Finally, it redirects to the Index action.
        public async Task<IActionResult> Upload(IFormFile file)
        {
            

            if (file != null && file.Length > 0)
            {
                await _azureFileService.UploadFileAsync(file);
                TempData["SuccessMessage"] = "File uploaded successfully!";

            }
            else
            {
                TempData["SuccessMessage"] = "NO FILE WAS RECEIVED";
            }

            
            return RedirectToAction(nameof(Index));
        }
    }
}
