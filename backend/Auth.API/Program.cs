using Auth.API.Api.Configurations;

await WebApplication.CreateBuilder(args)
   .ConfigureApplicationServices() // Faz todos os Adds (API + Infra)
   .Build()                        // Constrói o host
   .ConfigureApplicationPipeline(); // Faz todos os Uses/Maps e dá o Run