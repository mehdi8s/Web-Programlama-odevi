using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AIKesim.Controllers
{
    public class ImageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public ImageController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _environment = environment;
        }

        // GET: Image/Index
        public IActionResult Index()
        {
            return View();
        }

        // POST: Image/UploadImage
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return BadRequest("Lütfen bir resim dosyası seçin.");
                }

                // Dosya uzantısını kontrol et
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Sadece .jpg, .jpeg ve .png uzantılı dosyalar kabul edilir.");
                }

                // Maksimum dosya boyutunu kontrol et (örn: 5MB)
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    return BadRequest("Dosya boyutu 5MB'dan küçük olmalıdır.");
                }

                // Resmi yükle ve analiz et
                var result = await AnalyzeImageWithAPI(imageFile);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }

        // POST: Image/AnalyzeImageWithAPI
        private async Task<object> AnalyzeImageWithAPI(IFormFile imageFile)
        {
            // Resmi byte dizisine çevir
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            var base64Image = Convert.ToBase64String(imageBytes);

            // API bilgilerini al
            var apiKey = _configuration["RapidAPI:Key"];
            var apiHost = _configuration["RapidAPI:Host"];
            var apiEndpoint = _configuration["RapidAPI:Endpoint"];

            // HTTP isteğini oluştur
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(apiEndpoint),
                Headers =
                {
                    { "X-RapidAPI-Key", apiKey },
                    { "X-RapidAPI-Host", apiHost },
                },
                Content = new StringContent(JsonSerializer.Serialize(new { image = base64Image }))
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            // API'ye istek gönder
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Yanıtı oku ve dön
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(jsonResponse);
        }

        // POST: Image/SaveImage
        [HttpPost]
        public async Task<IActionResult> SaveImage(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    return BadRequest("Dosya yüklenemedi.");

                // Uploads klasörü yolu
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                // Klasör yoksa oluştur
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Benzersiz dosya adı oluştur
                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Dosyayı kaydet
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return Json(new { filename = uniqueFileName });
            }
            catch (Exception ex)
            {
                return BadRequest($"Dosya kaydedilemedi: {ex.Message}");
            }
        }

        // DELETE: Image/DeleteImage
        [HttpDelete]
        public IActionResult DeleteImage(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok("Dosya başarıyla silindi.");
                }

                return NotFound("Dosya bulunamadı.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Dosya silinemedi: {ex.Message}");
            }
        }
    }
}