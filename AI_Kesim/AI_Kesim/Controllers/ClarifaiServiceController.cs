using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public class ClarifaiService
{
    private readonly string _apiKey = "816d722f741f4dbfbbee3f4d4f1dfed3"; // API Anahtarınızı buraya ekleyin
    private readonly string _url = "https://api.clarifai.com/v2/models/face-detection/outputs";  // API URL'si

    // Metodun doğru şekilde tanımlandığından emin olun
    public async Task<string> UploadImageAndAnalyze(byte[] imageBytes)
    {
        using (var client = new HttpClient())
        {
            // Authorization header
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            // Görseli API'ye göndermek için multipart/form-data içeriği oluşturuluyor
            var content = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(imageBytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            content.Add(byteArrayContent, "file", "image.jpg");

            var response = await client.PostAsync(_url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }

            var errorData = await response.Content.ReadAsStringAsync();
            return $"Error: {response.StatusCode}, Details: {errorData}";
        }
    }
}