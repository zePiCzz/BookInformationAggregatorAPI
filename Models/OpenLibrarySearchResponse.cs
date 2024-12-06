using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Models
{
    public class OpenLibrarySearchResponse
    {
        [JsonPropertyName("docs")]
        public IEnumerable<OpenLibraryBook> Docs { get; set; }
    }

    public class OpenLibraryBook
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author_name")]
        public IEnumerable<string> AuthorName { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("edition_key")]
        public List<string> EditionKeys { get; set; }

    }
}
