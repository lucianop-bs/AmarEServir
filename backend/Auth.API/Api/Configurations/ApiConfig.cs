using AmarEServir.Core.Filters;
using AmarEServir.Core.Json.Converter;
using AmarEServir.Core.Middlewares;
using Auth.API.Application.Cells.CreateCell;
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
        // 1. APONTAMENTO DE ASSEMBLY 
        var applicationAssembly = typeof(CreateCellCommand).Assembly;

        // 2. Injeção de Dependência: FluentValidation e MediatR
        builder.Services.AddValidatorsFromAssembly(applicationAssembly);

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // 3. Tratamento de Exceções Global
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // 4. Configuração de Controllers e JSON
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiResultFilter>();
        })
        .AddJsonOptions(options =>
        {
            // Converte Enums (Admin -> "Admin")
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            // RESOLVE O ERRO 500: Adiciona o conversor de Guid que criamos acima
            options.JsonSerializerOptions.Converters.Add(new NullableGuidConverter());

            options.JsonSerializerOptions.AllowTrailingCommas = true;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            // Deixe em FALSE para que o ASP.NET use o Factory abaixo quando o Model estiver inválido
            options.SuppressModelStateInvalidFilter = false;

            options.InvalidModelStateResponseFactory = context =>
            {
                // Pega a primeira mensagem de erro dos validadores
                var errorMessage = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault() ?? "Dados de entrada inválidos.";

                return new BadRequestObjectResult(new { status = 400, message = errorMessage });
            };
        });

        // 5. Infraestrutura e Swagger
        builder.Services.AddOpenApi();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Total", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        return builder;
    }

    public static void ConfigureApplicationPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Amar e Servir API").WithTheme(ScalarTheme.Moon);
            });
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseCors("Total");
        app.UseAuthorization();
        app.MapControllers();
    }
}