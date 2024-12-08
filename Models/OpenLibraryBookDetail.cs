using BookInformationAggregatorAPI.Models.Converters;
using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Models
{
    public class OpenLibraryBookDetail
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("first_publish_date")]
        public string FirstPublishDate { get; set; }

        [JsonPropertyName("description")]
        [JsonConverter(typeof(DescriptionConverter))]
        public string Description { get; set; }

        [JsonPropertyName("authors")]
        public List<OpenLibraryAuthor> Authors { get; set; }

        [JsonPropertyName("covers")]
        public List<int> Covers { get; set; }

        [JsonPropertyName("subject_places")]
        public List<string> SubjectPlaces { get; set; }

        [JsonPropertyName("subject_people")]
        public List<string> SubjectPeople { get; set; }

        [JsonPropertyName("subjects")]
        public List<string> Subjects { get; set; }

        [JsonPropertyName("subject_times")]
        public List<string> SubjectTimes { get; set; }

        [JsonPropertyName("first_sentence")]
        [JsonConverter(typeof(FirstSentenceConverter))]
        public string FirstSentence { get; set; }
    }

    public class OpenLibraryAuthor
    {
        [JsonPropertyName("author")]
        public OpenLibraryAuthorDetail Author { get; set; }
    }

    public class OpenLibraryAuthorDetail
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}
