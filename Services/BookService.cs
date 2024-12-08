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
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);

                    // Attempt to deserialize the JSON data
                    _books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
                }
                else
                {
                    // Handle missing file scenario
                    _books = new List<Book>();
                    throw new FileNotFoundException($"The file '{_filePath}' does not exist.");
                }
            }
            catch (FileNotFoundException ex)
            {
                // Log the error and provide a user-friendly message
                throw new Exception($"Books data file not found: {ex.Message}.");
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing errors
                throw new Exception($"Failed to parse the books.json file. Ensure it contains valid JSON. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                throw new Exception($"An unexpected error occurred while initializing the BookService: {ex.Message}");
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
                PublishedYear = d.FirstPublishYear ?? 0,
                EditionKeys = d.EditionKeys
            }) ?? Enumerable.Empty<BookSearch>();
        }

        // Fetches detailed information about a book by its Open Library ID (key)
        public async Task<OpenLibraryBookDetail?> GetOpenLibraryBookDetailAsync(string id)
        {
            try
            {
                // Try fetching details from /works/{id}
                var workUrl = $"https://openlibrary.org/works/{id}.json";
                Console.WriteLine($"Attempting to fetch work details for ID: {id} from {workUrl}");

                var workResponse = await _httpClient.GetAsync(workUrl);

                if (workResponse.IsSuccessStatusCode)
                {
                    var workContent = await workResponse.Content.ReadAsStringAsync();
                    var workDetail = JsonSerializer.Deserialize<OpenLibraryBookDetail>(workContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (workDetail != null)
                    {
                        Console.WriteLine($"Work details fetched successfully for ID: {id}");
                        return workDetail;
                    }
                }
                else
                {
                    Console.WriteLine($"Work ID not found: {id}. Status Code: {workResponse.StatusCode}");
                }

                // If not found in /works, try /books
                var bookUrl = $"https://openlibrary.org/books/{id}.json";
                Console.WriteLine($"Attempting to fetch book details for ID: {id} from {bookUrl}");

                var bookResponse = await _httpClient.GetAsync(bookUrl);

                if (bookResponse.IsSuccessStatusCode)
                {
                    var bookContent = await bookResponse.Content.ReadAsStringAsync();
                    var bookDetail = JsonSerializer.Deserialize<OpenLibraryBookDetail>(bookContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (bookDetail != null)
                    {
                        Console.WriteLine($"Book details fetched successfully for ID: {id}");
                        return bookDetail;
                    }
                }
                else
                {
                    Console.WriteLine($"Book ID not found: {id}. Status Code: {bookResponse.StatusCode}");
                }

                // If neither works nor books endpoint has the data
                Console.WriteLine($"Details for ID {id} not found in either works or books endpoint.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching book details for ID {id}: {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}
