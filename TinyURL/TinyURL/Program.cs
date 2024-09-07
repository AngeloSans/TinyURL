using TinyURL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapPost("ap/short", (ShortenUrlRequest request) =>
{
    if (Uri.TryCreate(request.Url, UriKind.RelativeOrAbsolute, out _)) ;
}
);
app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
