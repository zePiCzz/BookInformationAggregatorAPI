﻿using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Models
{
    public class BookSearch
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
        public IEnumerable<string> Author { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("first_sentence")]
        public IEnumerable<string> FirstSentence { get; set; }

        [JsonPropertyName("format")]
        public IEnumerable<string> Format { get; set; }

    }
}
