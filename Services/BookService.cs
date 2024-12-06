using BookInformationAggregatorAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace BookInformationAggregatorAPI.Services
{
    public class BookService
    {
        private readonly string _filePath = "books.json";
        private readonly List<Book> _books;
        private readonly HttpClient _httpClient;

        #region Constructor and Initialization

        // Initializes the BookService by loading data from books.json
        public BookService()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            }
            else
            {
                _books = new List<Book>();
            }

            _httpClient = new HttpClient();
        }

        #endregion

        #region Local Data Operations

        // Returns all books from the in-memory list
        public IEnumerable<Book> GetAllBooks() => _books;

        // Returns a book by its ID
        public Book? GetBookById(string id) => _books.FirstOrDefault(book => book.Id == id);

        // Adds a new book to the in-memory list and saves to file
        public void AddBook(Book book)
        {
            _books.Add(book);
            SaveChanges();
        }

        // Deletes a book by its ID from the in-memory list and updates the file
        public bool DeleteBook(string id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
                SaveChanges();
                return true;
            }
            return false;
        }

        // Saves the in-memory book list to the books.json file
        private void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        #endregion

        #region External API Operations

        // Searches for books by title or author from the Open Library API
        public async Task<IEnumerable<BookSearch>> SearchBooksAsync(string query)
        {
            var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}";
            var response = await _httpClient.GetStringAsync(url);

            var searchResult = JsonSerializer.Deserialize<OpenLibrarySearchResponse>(response);

            return searchResult?.Docs.Select(d => new BookSearch
            {
                Id = d.Key.Replace("/works/", ""),
                Title = d.Title,
                Author = string.Join(", ", d.AuthorName ?? Array.Empty<string>()),
                PublishedYear = d.FirstPublishYear ?? 0
            }) ?? Enumerable.Empty<BookSearch>();
        }

        // Fetches detailed information about a book by its Open Library ID
        public async Task<OpenLibraryBookDetail?> GetOpenLibraryBookDetailAsync(string id)
        {
            try
            {
                var url = $"https://openlibrary.org/works/{id}.json";
                var response = await _httpClient.GetStringAsync(url);

                var bookDetail = JsonSerializer.Deserialize<OpenLibraryBookDetail>(response);

                if (bookDetail == null) return null;

                // Fetch author names dynamically
                if (bookDetail.Authors != null)
                {
                    foreach (var author in bookDetail.Authors)
                    {
                        var authorUrl = $"https://openlibrary.org{author.Author.Key}.json";
                        var authorResponse = await _httpClient.GetStringAsync(authorUrl);
                        var authorDetail = JsonSerializer.Deserialize<OpenLibraryAuthorDetail>(authorResponse);

                        author.Author.Key = authorDetail?.Key.Split('/').Last(); // Update to author name
                    }
                }

                return bookDetail;
            }
            catch (Exception)
            {
                // Handle errors gracefully
                return null;
            }
        }


        #endregion
    }
}
