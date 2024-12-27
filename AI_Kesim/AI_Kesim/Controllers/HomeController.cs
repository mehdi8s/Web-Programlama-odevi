using System.Diagnostics;
using AI_Kesim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;

namespace AI_Kesim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClarifaiService _clarifaiService;  // ClarifaiService'i DI ile ekliyoruz

        public HomeController(ILogger<HomeController> logger, ClarifaiService clarifaiService)
        {
            _logger = logger;
            _clarifaiService = clarifaiService;
        }

        // Ana sayfa
        public IActionResult Index()
        {
            return View();
        }

        // Foto?raf analiz etme
        [HttpPost]
        public async Task<IActionResult> Analyze(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                // Foto?raf? Clarifai API'ye göndererek analiz et
                var analysisResult = await _clarifaiService.UploadImageAndAnalyze(imageBytes);

                // API'den gelen sonucu kullan?c?ya göster
                ViewBag.AnalysisResult = analysisResult;
            }
            else
            {
                ViewBag.AnalysisResult = "No file selected.";
            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}