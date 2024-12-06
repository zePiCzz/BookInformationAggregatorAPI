using BookInformationAggregatorAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BookInformationAggregatorAPI.Services
{
    public class BookService
    {
        private readonly string _filePath = "books.json";
        private readonly List<Book> _books;

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
    }
}
