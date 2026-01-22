using Auth.API.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureApplicationServices()
    .Build();

app.ConfigureApplicationPipeline();

app.Run();