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
                // If the description is an object, try to parse its "value" property.
                using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                {
                    if (jsonDoc.RootElement.TryGetProperty("value", out var valueProp))
                    {
                        return valueProp.GetString();
                    }
                }
            }

            // Return a default description if none is available.
            return "No description available.";
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            // Write the description back as a string.
            writer.WriteStringValue(value);
        }
    }
}
