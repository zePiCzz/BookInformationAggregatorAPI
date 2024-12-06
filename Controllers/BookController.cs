using BookInformationAggregatorAPI.Models;
using BookInformationAggregatorAPI.Services;
using Microsoft.AspNetCore.Mvc;

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

        // GET /books/{id}
        // Returns a book by its ID
        [HttpGet("{id}")]
        public ActionResult<Book> GetBookById(string id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST /books
        // Adds a new book to the collection
        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            _bookService.AddBook(newBook);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
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
    }
}
