using Auth.API.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureApplicationServices()
    .Build();

app.ConfigureApplicationPipeline();

app.Run();