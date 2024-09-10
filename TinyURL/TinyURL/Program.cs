using Microsoft.EntityFrameworkCore;
using TinyURL;
using TinyURL.Entities;
using TinyURL.Models;
using TinyURL.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext and services **before** calling builder.Build()
//builder.Services.AddDbContext<ApplicationDbContext>(o =>
    //o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("api/shorten", async (
    ShortenUrlRequest request,
    UrlShorteningService urlShorteningService,
    ApplicationDbContext dbContext,
    HttpContext httpContext) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("The specified URL is invalid");
    }

    string code;

    if (!string.IsNullOrEmpty(request.Alias))
    {
        
        var aliasExists = await dbContext.ShortenedURLs.AnyAsync(s => s.Code == request.Alias);
        if (aliasExists)
        {
            return Results.BadRequest("Alias already in use. Please choose a different alias.");
        }
        code = request.Alias;
    }
    else
    {
       
        code = await urlShorteningService.GenerateUniqueCode();
    }

    var shortenedUrl = new ShortenedURL
    {
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        CreatedOnUtc = DateTime.UtcNow
    };

    dbContext.ShortenedURLs.Add(shortenedUrl);
    await dbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);
});


app.MapGet("api/{code}", async (string code, ApplicationDbContext dbContext) =>
{
    var shortenedUrl = await dbContext.ShortenedURLs
        .FirstOrDefaultAsync(s => s.Code == code);

    if (shortenedUrl == null)
    {
        return Results.NotFound();
    }
    return Results.Redirect(shortenedUrl.LongUrl);
});

app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

app.Run();
