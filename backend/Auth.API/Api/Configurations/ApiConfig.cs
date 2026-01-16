using AmarEServir.Core.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

namespace Auth.API.Api.Configurations
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder ConfigureApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
            {

                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

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
                });
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
