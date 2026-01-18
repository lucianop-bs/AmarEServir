using Auth.API.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Auth.API.Infrastructure.Persistence.Mapping
{
    public static class MongoDbMapping
    {
        public static void Configure()
        {

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            // 2. Pacote de Convenções
            var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(), // Transforma "NomeUsuario" em "nomeUsuario" no banco
            new IgnoreExtraElementsConvention(true), // Se o banco tiver campos a mais, o C# não quebra
            new IgnoreIfNullConvention(true), // Se a propriedade for nula, não salva no banco (economiza espaço)
            new EnumRepresentationConvention(BsonType.String) // Salva Enums como texto ("Ativo") e não número (1)
        };
            var entityNameSpace = typeof(User).Namespace;
            // Registra as convenções para todas as classes do seu namespace de Entidades
            ConventionRegistry.Register("AmarEServirConventions", pack, t => t.FullName!.StartsWith(entityNameSpace!));
        }

        public static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(u => u.Id); // Define explicitamente qual campo é a Chave Primária


                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Cell)))
            {
                BsonClassMap.RegisterClassMap<Cell>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(c => c.Id); // Define explicitamente qual campo é a Chave Primária
                    cm.MapMember(c => c.LeaderId)
                    .SetIsRequired(true);

                    cm.UnmapProperty(c => c.Users);

                });
            }
        }
    }
}
