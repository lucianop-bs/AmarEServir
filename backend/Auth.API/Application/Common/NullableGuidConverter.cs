using System.Text.Json;
using System.Text.Json.Serialization;

namespace Auth.API.Api.Configurations;

public class NullableGuidConverter : JsonConverter<Guid?>
{
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Se o que veio no JSON for uma string
        if (reader.TokenType == JsonTokenType.String)
        {
            string? value = reader.GetString();

            // SEGREDO: Se for vazio ou espaços, retornamos NULL para o C#
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // Se não for vazio, tenta converter normalmente
            if (Guid.TryParse(value, out var result))
                return result;
        }

        // Se o valor no JSON for explicitamente null (sem aspas)
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value); 
        }
        else writer.WriteNullValue();
    }
}