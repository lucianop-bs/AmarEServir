using Auth.API.Domain.Contracts;
using Auth.API.Infrastructure.Persistence.Context;
using Auth.API.Infrastructure.Persistence.Mapping;
using Auth.API.Infrastructure.Persistence.Repositories;
using MongoDB.Driver;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Importante: Não esqueça de chamar o mapeamento do Bson
        MongoDbMapping.Configure();

        var settings = configuration.GetSection("MongoDbSettings");

        // 1. Cliente (Singleton)
        services.AddSingleton<IMongoClient>(sp => new MongoClient(settings["ConnectionString"]));

        // 2. Banco (Scoped)
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings["DatabaseName"] ?? "AmarEServirDB");
        });

        // 3. Contexto (Scoped)
        services.AddScoped<MongoContext>();

        // --- ADICIONE ESTAS LINHAS ABAIXO ---
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICellRepository, CellRepository>();
        // ------------------------------------

        return services;
    }
}