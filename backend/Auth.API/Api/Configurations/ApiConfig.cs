using AmarEServir.Core.Filters;
using AmarEServir.Core.Middlewares;
using Auth.API.Application.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

namespace Auth.API.Api.Configurations;

public static class ApiConfig
{
    public static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(Program).Assembly;

        // 1. Injeção de Dependência: MediatR e Validation
        builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // 2. Tratamento de Exceções Global
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // 3. Configuração de Controllers e JSON
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiResultFilter>();
        })
        .AddJsonOptions(options =>
        {
            // Converte Enums para String no JSON (ex: 0 vira "Admin")
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // Desativa a validação automática do ASP.NET para usarmos o nosso ValidationBehavior
            options.SuppressModelStateInvalidFilter = true;

            // Customiza a resposta caso algo ainda caia na validação do Model do ASP.NET
            options.InvalidModelStateResponseFactory = context =>
            {
                var errorMessage = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault() ?? "Dados de entrada inválidos.";

                return new BadRequestObjectResult(new { status = 400, message = errorMessage });
            };
        });

        // 4. Documentação e Infraestrutura
        builder.Services.AddOpenApi();
        builder.Services.AddInfrastructure(builder.Configuration);

        // 5. Segurança (CORS)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Total", b =>
                b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        return builder;
    }

    public static void ConfigureApplicationPipeline(this WebApplication app)
    {
        // 1. Ambiente de Desenvolvimento (Documentação)
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Amar e Servir API")
                       .WithTheme(ScalarTheme.Moon)
                       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }

        // 2. Middlewares de Fluxo (Ordem é importante!)
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseCors("Total");
        app.UseAuthorization();

        // 3. Mapeamento de Rotas
        app.MapControllers();
    }
}