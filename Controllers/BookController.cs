using BookInformationAggregatorAPI.Models;
using BookInformationAggregatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace BookInformationAggregatorAPI.Controllers
{
    [ApiController]
    [Route("books")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        #region Endpoints for Local Data

        // GET /books
        // Returns all books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAllBooks()
        {
            return Ok(_bookService.GetAllBooks());
        }

        // POST /books
        // Adds a new book to the collection
        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            _bookService.AddBook(newBook);
            return Created($"/books/{newBook.Id}", newBook);
        }

        // DELETE /books/{id}
        // Deletes a book by its ID
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(string id)
        {
            var result = _bookService.DeleteBook(id);
            if (!result) return NotFound(new { message = "Book not found" });

            return NoContent();
        }

        #endregion

        #region Endpoints for External API Operations

        // GET /books/search?query={query}
        // Searches for books by title or author
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookSearch>>> SearchBooks([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest(new { message = "Query parameter is required" });

            var books = await _bookService.SearchBooksAsync(query);
            return Ok(books);
        }

        // GET /books/{id}
        // Fetches detailed information about a book by its Open Library ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OpenLibraryBookDetail>> GetBookDetails(string id)
        {
            Console.WriteLine($"Fetching book details for ID: {id}");

            var bookDetail = await _bookService.GetOpenLibraryBookDetailAsync(id);

            if (bookDetail == null)
            {
                Console.WriteLine($"Book details not found for ID: {id}");
                return NotFound(new { message = $"Book with ID {id} not found." });
            }

            Console.WriteLine($"Book details successfully fetched for ID: {id}");
            return Ok(bookDetail);
        }
        #endregion
    }
}
