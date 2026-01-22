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
            // ✅ Force UTC para todas as datas
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Utc));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            // ✅ Serializer para DateTime nullable
            BsonSerializer.RegisterSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Utc)));

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

            if (!BsonClassMap.IsClassMapRegistered(typeof(RefreshToken)))
            {
                BsonClassMap.RegisterClassMap<RefreshToken>(cm =>
                {
                    cm.MapCreator(c => new RefreshToken());

                    cm.MapMember(c => c.Token)
                      .SetElementName("token")
                      .SetIsRequired(false);

                    cm.MapMember(c => c.Created)
                      .SetElementName("created")
                      .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc))
                      .SetIsRequired(true);

                    cm.MapMember(c => c.Expires)
                      .SetElementName("expires")
                      .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc))
                      .SetIsRequired(true);

                    cm.MapMember(c => c.Revoked)
                      .SetElementName("revoked")
                      .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Utc)))
                      .SetIsRequired(false);

                    cm.UnmapProperty(c => c.IsExpired);
                    cm.UnmapProperty(c => c.IsRevoked);
                    cm.UnmapProperty(c => c.IsActive);
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