using BookInformationAggregatorAPI.Models;
using BookInformationAggregatorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
        // Returns all books from the collection
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAllBooks()
        {
            // Fetch all books from the service
            var books = _bookService.GetAllBooks();

            // Handle case where no books are found
            if (!books.Any())
            {
                return NotFound(new
                {
                    message = "No books available in the collection.",
                });
            }

            // Return the list of books with success message
            return Ok(new
            {
                message = "Books retrieved successfully.",
                count = books.Count(),
                books = books
            });
        }

        // POST /books
        // Adds a new book to the collection
        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            try
            {
                // Validate the incoming book data
                if (newBook == null)
                {
                    return BadRequest(new
                    {
                        message = "Invalid book data. The request body cannot be null.",
                    });
                }

                if (string.IsNullOrWhiteSpace(newBook.Id) ||
                    string.IsNullOrWhiteSpace(newBook.Title) ||
                    string.IsNullOrWhiteSpace(newBook.Author))
                {
                    return BadRequest(new
                    {
                        message = "Invalid book data. 'Id', 'Title', and 'Author' fields are required.",
                    });
                }

                // Check if the book ID already exists
                var existingBook = _bookService.GetAllBooks().FirstOrDefault(b => b.Id == newBook.Id);
                if (existingBook != null)
                {
                    return Conflict(new
                    {
                        message = $"A book with ID '{newBook.Id}' already exists.",
                    });
                }

                // Add the book to the collection
                _bookService.AddBook(newBook);
                return Created($"/books/{newBook.Id}", new
                {
                    message = "Book added successfully.",
                    book = newBook
                });
            }
            catch (Exception ex)
            {
                // General error handling
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while adding the book.",
                    errorDetails = ex.Message
                });
            }
        }

        // DELETE /books/{id}
        // Deletes a book by its ID
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(string id)
        {
            try
            {
                // Validate the ID
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new
                    {
                        message = "Invalid ID. The book ID cannot be null or empty.",
                    });
                }

                // Attempt to delete the book
                var result = _bookService.DeleteBook(id);
                if (!result)
                {
                    return NotFound(new
                    {
                        message = $"Book with ID '{id}' not found.",
                    });
                }

                // Return success response
                return Ok(new
                {
                    message = $"Book with ID '{id}' has been successfully deleted."
                });
            }
            catch (Exception ex)
            {
                // General error handling
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while deleting the book.",
                    errorDetails = ex.Message
                });
            }
        }
        #endregion

        #region Endpoints for External API Operations

        // GET /books/search?query={query}
        // Searches for books by title or author
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookSearch>>> SearchBooks([FromQuery] string query)
        {
            // Validate query parameter
            if (string.IsNullOrEmpty(query))
                return BadRequest(new { message = "Query parameter is required" });

            // Fetch search results
            var books = await _bookService.SearchBooks(query);

            return Ok(books);
        }

        // GET /books/{id}
        // Fetches detailed information about a book by its Open Library ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OpenLibraryBookDetail>> GetBookDetails(string id)
        {
            // Fetch book details by ID
            var bookDetail = await _bookService.GetOpenLibraryBookDetail(id);

            // Return 404 if not found
            if (bookDetail == null)
                return NotFound(new { message = $"Book with ID {id} not found." });

            return Ok(bookDetail);
        }
        #endregion
    }
}
