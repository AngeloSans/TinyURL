using Microsoft.EntityFrameworkCore;

namespace TinyURL.Service
{
    public class UrlShorteningService
    {
        public const int NumberOfCharInShortLink = 7;
        private const string _Alphabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

        private readonly Random _random = new();
        private readonly ApplicationDbContext _dbContext;

        public async Task<string> GenerateUniqueCode()
        {
            var codeChars = new char[NumberOfCharInShortLink];

            while(true)
            {
                for (int i = 0; i < NumberOfCharInShortLink; i++)
                {
                    var randomIndex = _random.Next(_Alphabet.Length - 1);

                    codeChars[i] = _Alphabet[randomIndex];
                }
                var code = new string(codeChars);

                if (!await _dbContext.ShortenedURLs.AnyAsync(s => s.Code == code))
                {
                    return code;
                }
            }

            
        }
    }
}
