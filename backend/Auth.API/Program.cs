using Auth.API.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura os serviços e constrói o App
var app = builder
    .ConfigureApplicationServices()
    // Retorna o builder
    .Build();                       // Transforma o builder em app

// 2. Configura o pipeline de execução
app.ConfigureApplicationPipeline(); // Configura middlewares (CORS, Swagger, etc)

// 3. Roda a aplicação
app.Run();