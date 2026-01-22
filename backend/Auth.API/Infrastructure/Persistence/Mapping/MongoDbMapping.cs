using AmarEServir.Core.Entities;
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
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfNullConvention(true),
                new EnumRepresentationConvention(BsonType.String)
            };

            var entityNameSpace = typeof(User).Namespace;
            ConventionRegistry.Register("AmarEServirConventions", pack, t => t.FullName!.StartsWith(entityNameSpace!));

            RegisterClassMaps();
        }

        public static void RegisterClassMaps()
        {
            // O mapeamento do RefreshToken deve vir antes
            if (!BsonClassMap.IsClassMapRegistered(typeof(RefreshToken)))
            {
                BsonClassMap.RegisterClassMap<RefreshToken>(cm =>
                {
                    cm.MapCreator(c => new RefreshToken());

                    cm.MapMember(c => c.Token).SetElementName("token");
                    cm.MapMember(c => c.Expires).SetElementName("expires");
                    cm.MapMember(c => c.Created).SetElementName("created");
                    cm.MapMember(c => c.Revoked).SetElementName("revoked");
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity<Guid>)))
            {
                BsonClassMap.RegisterClassMap<BaseEntity<Guid>>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(u => u.Id);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();

                    cm.MapField("_refreshTokens").SetElementName("refresh_tokens");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Cell)))
            {
                BsonClassMap.RegisterClassMap<Cell>(cm =>
                {
                    cm.AutoMap();

                    cm.MapMember(c => c.LeaderId).SetIsRequired(true);
                    cm.UnmapProperty(c => c.Members);
                });
            }
        }
    }
}