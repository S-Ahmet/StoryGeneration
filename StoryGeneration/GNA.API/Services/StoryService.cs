using System.Text;
using System.Text.Json;
using GNA.API.Interfaces;
using GNA.API.Requests;
using GNA.API.Responses;

namespace GNA.API.Services
{
    public class StoryService : IStoryService
    {
        private readonly HttpClient _httpClient;

        public StoryService(IHttpClientFactory httpClientFactory)
        {
            // ✅ DOĞRU CLIENT OLUŞTURMA
            _httpClient = httpClientFactory.CreateClient();

            // ⏱️ LLM çağrıları uzun sürebilir
            _httpClient.Timeout = TimeSpan.FromMinutes(5);

            // 🧱 Ngrok için gerekli header
            _httpClient.DefaultRequestHeaders.Add(
                "ngrok-skip-browser-warning", "true"
            );
        }

        public async Task<StoryResponse> HikayeUret(StoryRequest request)
        {
            string apiUrl =
                "https://keyla-curricular-scurrilously.ngrok-free.dev/generate";

            try
            {
                // 🔥 COLAB UYUMLU PAYLOAD
                var colabRequest = new
                {
                    Tema = request.Tema,
                    Mekan = request.Mekan,
                    Zaman = request.Zaman,
                    BaslangicCumlesi = request.BaslangicCumlesi
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(colabRequest),
                    Encoding.UTF8,
                    "application/json"
                );

                Console.WriteLine("➡️ Colab isteği gönderiliyor...");

                var response = await _httpClient.PostAsync(apiUrl, jsonContent);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine("⬅️ Colab cevabı alındı.");

                if (!response.IsSuccessStatusCode)
                {
                    return new StoryResponse
                    {
                        Basarili = false,
                        HataMesaji = $"Colab Hatası ({response.StatusCode}): {content}"
                    };
                }

                return JsonSerializer.Deserialize<StoryResponse>(
                    content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                )!;
            }
            catch (Exception ex)
            {
                // 🔥 GERÇEK HATAYI GÖR
                Console.WriteLine("🔥 HTTP ERROR:");
                Console.WriteLine(ex.ToString());

                return new StoryResponse
                {
                    Basarili = false,
                    HataMesaji = ex.Message
                };
            }
        }
    }
}
