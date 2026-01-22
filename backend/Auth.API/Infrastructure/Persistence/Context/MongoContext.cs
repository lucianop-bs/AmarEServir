using MongoDB.Bson;
using MongoDB.Driver;

namespace Auth.API.Infrastructure.Persistence.Context;

public class MongoContext
{
    public IMongoDatabase Database { get; }
    public IMongoClient Client { get; }

    public MongoContext(IMongoClient client, IMongoDatabase database)
    {
        Client = client;
        Database = database;

        VerifyConnection();
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return Database.GetCollection<T>(name);
    }

    private void VerifyConnection()
    {
        try
        {
            Database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
        }
        catch (Exception ex)
        {
            throw new Exception($">>> Erro crítico de conexão no Atlas: {ex.Message}");
        }
    }
}