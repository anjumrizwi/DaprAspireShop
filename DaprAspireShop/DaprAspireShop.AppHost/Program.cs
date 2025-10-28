using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DaprAspireShop_ProductAPI>("dapraspireshop-productapi")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "dapraspireshop-productapi",
    });

builder.AddProject<Projects.DaprAspireShop_CartAPI>("dapraspireshop-cartapi")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "dapraspireshop-cartapi",
    });

builder.Build().Run();
