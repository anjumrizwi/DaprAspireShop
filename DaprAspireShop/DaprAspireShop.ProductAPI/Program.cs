using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

var products = new List<string>
{
    "Laptop",
    "Keyboard",
    "Mouse"
};

app.MapGet("/products", () =>
{
    var products = new List<string> { "Laptop", "Keyboard", "Mouse" };
    return Results.Ok(new
    {
        Products = products,
        CreatedAt = DateTime.Now
    });
});

// Keep existing POST if you need it
app.MapPost("/products", ([FromBody] ProductList request) =>
{
    return Results.Ok(new
    {
        Message = "Products added successfully",
        Count = request.Products.Count,
        AddedAt = DateTime.Now
    });
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record ProductList(List<string> Products);