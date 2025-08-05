// Minimal API Back-End (ServerApp.cs):

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors();
var app = builder.Build();

// Use CORS
app.UseCors(policy =>
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
);

app.MapGet("/api/productlist", () =>
{
    return new[]
    {
        new
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200.50,
            Stock = 25,
            Category = new { Id = 101, Name = "Electronics" }
        },
        new
        {
            Id = 2,
            Name = "Headphones",
            Price = 50.00,
            Stock = 100,
            Category = new { Id = 102, Name = "Accessories" }
        }
    };
});

app.Run();