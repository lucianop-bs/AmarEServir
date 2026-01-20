using AmarEServir.Core.Filters;
using AmarEServir.Core.Middlewares;
using Auth.API.Application.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

namespace Auth.API.Api.Configurations
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)

        {
            var applicationAssembly = typeof(Program).Assembly;
            builder.Services.AddValidatorsFromAssembly(applicationAssembly);

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
            }
            ).ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {

                        var errorMessage = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault() ?? "Dados de entrada inválidos.";

                        var result = new
                        {
                            status = StatusCodes.Status400BadRequest,
                            message = errorMessage
                        };

                        return new BadRequestObjectResult(result);
                    };
                })
            .AddJsonOptions(options =>
            {

                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }); ;
            builder.Services.AddOpenApi();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Total", b =>
                    b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            return builder;
        }

        public static async Task ConfigureApplicationPipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTitle("Amar e Servir API")
                        .WithTheme(ScalarTheme.Moon)
                        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseCors("Total");
            app.UseAuthorization();
            app.MapControllers();
            await app.RunAsync();
        }
    }
}
