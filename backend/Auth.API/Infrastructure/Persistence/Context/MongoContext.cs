using MongoDB.Bson;
using MongoDB.Driver;

namespace Auth.API.Infrastructure.Persistence.Context;

public class MongoContext
{
    public IMongoDatabase Database { get; }
    public IMongoClient Client { get; }

    // Agora ela recebe o cliente e o banco já configurados no DependencyInjection.cs
    public MongoContext(IMongoClient client, IMongoDatabase database)
    {
        Client = client;
        Database = database;

        // Mantemos sua verificação, mas apenas como log de inicialização
        VerifyConnection();
    }

    // Facilitador para pegar coleções sem precisar digitar o nome toda hora
    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return Database.GetCollection<T>(name);
    }

    private void VerifyConnection()
    {
        try
        {
            Database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            // Dica: Use um Logger em vez de Console.WriteLine em produção
        }
        catch (Exception ex)
        {
            throw new Exception($">>> Erro crítico de conexão no Atlas: {ex.Message}");
        }
    }
}