using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

// 🧱 Shared Dapr component defaults
var daprOptions = new DaprSidecarOptions
{
    DaprHttpPort = 3500, // Aspire assigns unique ones automatically, this is a default
    DaprGrpcPort = 50001,
    AppProtocol = "http",
    LogLevel = "info"
};

// 🛒 Product API
var productApi = builder.AddProject<Projects.DaprAspireShop_ProductAPI>("dapraspireshop-productapi")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "dapraspireshop-productapi",
        AppPort = 5001
    })
    // ⛔ Disable parallel build to prevent DLL lock
    .WithEnvironment("DOTNET_HOST_BUILD_DISABLE_PARALLEL", "true")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

// 🧺 Cart API
var cartApi = builder.AddProject<Projects.DaprAspireShop_CartAPI>("dapraspireshop-cartapi")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "dapraspireshop-cartapi",
        AppPort = 5002
    })
    // 👇 Same protection against file lock
    .WithEnvironment("DOTNET_HOST_BUILD_DISABLE_PARALLEL", "true")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

// ✅ Optional: set dependency ordering to avoid concurrent builds
cartApi.WithReference(productApi);

builder.Build().Run();
