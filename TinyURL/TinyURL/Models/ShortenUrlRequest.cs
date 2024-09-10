namespace TinyURL.Models
{
    public class ShortenUrlRequest
    {
        public string Url { get; set; } = string.Empty;
        public string? Alias {  get; set; } 
    }
}
