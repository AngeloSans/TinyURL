namespace TinyURL.Entities
{
    public class ShortenedURL
    {
        public Guid Id { get; set; }

        public String LongUrl {  get; set; } = string.Empty;

        public string ShortUrl {  get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public DateTime CreatedOnUtc { get; set; }
    }
}
