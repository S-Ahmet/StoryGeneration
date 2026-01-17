namespace GNA.API.Models
{
    public class StoryResult
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Story { get; set; } = string.Empty;

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
