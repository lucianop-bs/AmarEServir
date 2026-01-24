using AmarEServir.Core.Filters;
using AmarEServir.Core.Json.Converter;
using AmarEServir.Core.Middlewares;
using AmarEServir.Core.Results.Api;
using Auth.API.Application.Cells.CreateCell;
using Auth.API.Application.Common;
using Auth.API.Application.Services;
using FluentValidation;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

namespace Auth.API.Api.Configurations;

public static class ApiConfig
{
    public static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(CreateCellCommand).Assembly;

        builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiResultFilter>();
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            options.JsonSerializerOptions.Converters.Add(new NullableGuidConverter());

            options.JsonSerializerOptions.AllowTrailingCommas = true;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = false;

            options.InvalidModelStateResponseFactory = context =>
            {
                var apiErrors = context.ModelState
                    .Where(ms => ms.Value?.Errors.Count > 0)
                    .SelectMany(ms => ms.Value!.Errors.Select(error =>
                        new ApiInfo(
                            Code: "400",
                            Message: error.ErrorMessage

                        )))
                    .ToList();

                var response = new
                {
                    errors = apiErrors
                };

                return new BadRequestObjectResult(response);
            };
        });

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Insira o token JWT no formato: seu_token"
                };

                document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();
                document.SecurityRequirements.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                return Task.CompletedTask;
            });
        });
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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}