using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Models
{
    public class Book
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("published_year")]
        public int PublishedYear { get; set; }
    }
}
