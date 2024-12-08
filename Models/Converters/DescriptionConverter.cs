using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Models.Converters
{
    public class DescriptionConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                // If the description is a string, return it directly.
                return reader.GetString();
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                // If the description is an object, parse its value property.
                using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                {
                    if (jsonDoc.RootElement.TryGetProperty("value", out var valueProp))
                    {
                        return valueProp.GetString();
                    }
                }
            }

            return "No description available.";
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
