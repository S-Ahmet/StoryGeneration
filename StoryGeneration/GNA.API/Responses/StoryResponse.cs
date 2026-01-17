namespace GNA.API.Responses
{
    public class StoryResponse
    {
        public string Baslik { get; set; } = string.Empty;
        public string Ozet { get; set; } = string.Empty;
        public string HikayeMetni { get; set; } = string.Empty;

        public bool Basarili { get; set; }
        public string? HataMesaji { get; set; }
    }
}
