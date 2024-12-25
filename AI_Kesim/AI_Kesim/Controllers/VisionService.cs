using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class VisionController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AnalyzeImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Lütfen bir görsel yükleyin.");
            }

            try
            {
                // Görseli bellek üzerine yükleyin
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // Google Vision API istemcisi oluştur
                var client = ImageAnnotatorClient.Create();
                var image = Image.FromStream(memoryStream);

                // Görseldeki etiketleri algıla
                var response = await client.DetectLabelsAsync(image);  // Doğru metot burada DetectLabelsAsync

                // Sonuçları döndür
                var labels = response.Select(l => new
                {
                    Description = l.Description,
                    Score = l.Score
                }).ToList();

                return View(labels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        // Görseli yüklemek için GET işlemi
        public IActionResult Index()
        {
            return View();
        }
    }
}
