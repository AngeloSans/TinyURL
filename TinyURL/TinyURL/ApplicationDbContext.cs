using Microsoft.EntityFrameworkCore;
using TinyURL.Entities;
using TinyURL.Service;

namespace TinyURL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<ShortenedURL> ShortenedURLs { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedURL>(builder =>
            {
                builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharInShortLink);
                builder.HasIndex(s => s.Code).IsUnique();
            });
        }
    }
}
