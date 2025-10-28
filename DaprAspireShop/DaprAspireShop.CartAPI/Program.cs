using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// ✅ Register Dapr client here
builder.Services.AddDaprClient();

builder.Services.AddControllers().AddDapr();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapControllers();
app.UseCloudEvents();
app.MapSubscribeHandler();

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

// ✅ Map your endpoint that uses Dapr
app.MapGet("/cart/add", async ([FromServices] DaprClient client) =>
{
    try
    {
        var products = await client.InvokeMethodAsync<List<string>>(
            HttpMethod.Get,                      // ✅ specify GET
            "dapraspireshop-productapi",
            "products");

        // Logic to add a product to the cart
        return Results.Ok(new
        {
            Message = "Product added to cart",
            Products = products
        });
    }
    catch (Exception ex)
    {
        // Return error details to help diagnose Dapr invocation problems
        return Results.Problem(detail: ex.ToString(), title: "Dapr invocation failed");
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
