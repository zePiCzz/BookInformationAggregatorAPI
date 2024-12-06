using System.Collections.Generic;
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
        public OpenLibraryDescription Description { get; set; }

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

        [JsonPropertyName("first_sentence")]
        public OpenLibraryFirstSentence FirstSentence { get; set; }
    }

    public class OpenLibraryDescription
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
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

    public class OpenLibraryFirstSentence
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
