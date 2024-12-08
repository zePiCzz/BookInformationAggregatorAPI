using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Models.Converters
{
    public class FirstSentenceConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                // If first_sentence is an object parse the value
                using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                {
                    if (jsonDoc.RootElement.TryGetProperty("value", out var valueProp))
                    {
                        return valueProp.GetString();
                    }
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                // If first_sentence is a simple string, return it directly
                return reader.GetString();
            }

            return "No first sentence available.";
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
