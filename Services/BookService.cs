using BookInformationAggregatorAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookInformationAggregatorAPI.Services
{
    public class BookService
    {
        private readonly List<Book> _books;

        public BookService()
        {
            var json = File.ReadAllText("books.json");
            _books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }

        public IEnumerable<Book> GetAllBooks() => _books;

        public Book? GetBookById(string id) => _books.FirstOrDefault(book => book.Id == id);

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public bool DeleteBook(string id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
                return true;
            }
            return false;
        }
    }
}
