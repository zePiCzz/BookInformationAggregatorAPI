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

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAllBooks()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBookById(string id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            _bookService.AddBook(newBook);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(string id)
        {
            var result = _bookService.DeleteBook(id);
            if (!result) return NotFound(new { message = "Book not found" });

            return NoContent(); // Return 204 if the deletion is successful
        }

    }
}
