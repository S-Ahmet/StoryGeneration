using System.Text;
using System.Text.Json;
using GNA.API.Interfaces;
using GNA.API.Requests;

namespace GNA.API.Services
{
    public class McpService : IMcpService
    {
        public string HikayePromptOlustur(StoryRequest request)
        {
            var prompt = new StringBuilder();

            prompt.AppendLine("# Hikaye Uretim Gorevi");
            prompt.AppendLine();
            prompt.AppendLine("## Baglam Verisi");
            prompt.AppendLine("```json");

            prompt.AppendLine(JsonSerializer.Serialize(new
            {
                tema = request.Tema,
                mekan = request.Mekan,
                zaman = request.Zaman,
                baslangicCumlesi = request.BaslangicCumlesi,
                ekIstek = request.EkIstek,
                hedefKelimeSayisi = request.HedefKelimeSayisi
            }, new JsonSerializerOptions { WriteIndented = true }));

            prompt.AppendLine("```");
            prompt.AppendLine();

            prompt.AppendLine("## Talimatlar");
            prompt.AppendLine("Sen yaratıcı ve tutarlı Türkçe hikayeler yazan bir yazarsın.");
            prompt.AppendLine("Verilen tema, mekan ve zaman bilgisini hikayeye yedir.");
            if (!string.IsNullOrWhiteSpace(request.BaslangicCumlesi))
                prompt.AppendLine("Hikayeye verilen başlangıç cümlesiyle başla.");
            prompt.AppendLine($"Hikaye yaklaşık {request.HedefKelimeSayisi} kelime olsun.");
            prompt.AppendLine("Hikaye akıcı olsun, olay örgüsü kopmasın, gereksiz tekrar yapma.");
            prompt.AppendLine();

            prompt.AppendLine("## Cikti Formati (Kesin)");
            prompt.AppendLine("Baslik: ...");
            prompt.AppendLine("Ozet: ...");
            prompt.AppendLine("Hikaye: ...");

            return prompt.ToString();
        }
    }
}
