using Auth.API.Domain.Contracts;
using Auth.API.Infrastructure.Persistence.Context;
using Auth.API.Infrastructure.Persistence.Mapping;
using Auth.API.Infrastructure.Persistence.Repositories;
using MongoDB.Driver;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDbMapping.Configure();

        var settings = configuration.GetSection("MongoDbSettings");

        services.AddSingleton<IMongoClient>(sp => new MongoClient(settings["ConnectionString"]));

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings["DatabaseName"] ?? "AmarEServirDB");
        });

        services.AddScoped<MongoContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICellRepository, CellRepository>();

        return services;
    }
}