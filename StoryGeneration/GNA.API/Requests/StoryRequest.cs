namespace GNA.API.Requests
{
    public class StoryRequest
    {
        public string Tema { get; set; } = string.Empty;
        public string Mekan { get; set; } = string.Empty;
        public string Zaman { get; set; } = string.Empty;

        public string BaslangicCumlesi { get; set; } = string.Empty;
        public string EkIstek { get; set; } = string.Empty;

        public int HedefKelimeSayisi { get; set; } = 150;
    }
}
