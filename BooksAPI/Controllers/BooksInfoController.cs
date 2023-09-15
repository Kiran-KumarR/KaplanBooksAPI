using Microsoft.AspNetCore.Mvc;
using BooksAPI.Controllers;
using BooksAPI.Models;
using BooksAPI.Interface;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksInfoController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IBooksInfoService _bookService;
      

        public BooksInfoController(IConfiguration configuration, IBooksInfoService bookService)
        {
            _configuration = configuration;
            _bookService = bookService;
        
        }

        /// <summary>
        /// GetAllBooks implements the HttpGet Method
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>

        // GET: api/<BooksInfoController>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] bool seed = true)
        {
            if (seed)
            {
                return await GetAllBooksWhenSeeding();//if seed if true
            }
            else
            {
                return await GetAllBooksWhenNotSeeding();//if seed is false
            }
        }
        private async Task<IActionResult> GetAllBooksWhenSeeding()
        {
            var booksFromDatabase = _bookService.RetrieveBooksFromDatabase();

            if (booksFromDatabase.Count == 0)
            {
                // Database is empty, try fetching from API
                var booksFromApi = await _bookService.FetchBooksFromApiAsync();

                if (booksFromApi != null && booksFromApi.Count > 0)
                {
                    await _bookService.StoreBooksInDatabase(booksFromApi);
                    return Ok(booksFromApi);
                }
                else
                {
                    // API failed, try fetching from local JSON file
                    var jsonFile = @"C:\Users\KKumarR\Desktop\BooksAPI\BooksAPI\Database\kaplan_book.json";
                    var booksFromJson = await _bookService.RetrieveBooksFromJson(jsonFile);

                    if (booksFromJson != null && booksFromJson.Count > 0)
                    {
                        await _bookService.StoreBooksInDatabase(booksFromJson);
                        return Ok(booksFromJson);
                    }
                    else
                    {
                        return NotFound("No books found from API or local JSON file.");
                    }
                }
            }
            else
            {
                // Database is not empty, return records from the database
                return Ok(booksFromDatabase);
            }
        }
       private async Task<IActionResult> GetAllBooksWhenNotSeeding()
        {
            var booksFromDatabase = _bookService.RetrieveBooksFromDatabase();

            if (booksFromDatabase.Count == 0)
            {
                return NotFound("No records found in the database.");
            }
            else
            {
                // Database is not empty, return records from the database
                return Ok(booksFromDatabase);
            }
        }


        /// <summary>
        /// GetBooks(int id)  implements [HttpGet("{id}")] Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<BooksInfoController>/5
        [HttpGet("{id}")]
        public IActionResult GetBooks(int id)   //to GET MEthod
        {

            var book = _bookService.GetBookById(id);    

            if (book != null)
            {
                return Ok(book);
            }
            else
            {
                return NotFound($"Book with ID {id} not found.");
            }
        }

        /// <summary>
        /// PostBooks implements [HttpPost] Method
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>

        // POST api/<BooksInfoController>
        [HttpPost]
        public IActionResult PostBooks(BookInfoModel bookInfo)
        {
            var postResult = _bookService.PostintoBooksTable(bookInfo);
            if (postResult != null)
            {
                return Ok(postResult);
            }
            else
            {
                return NoContent();
            }
            //return _bookService.PostBooks( bookInfo).ToList();
        }

        /// <summary>
        /// PutintoBooks implements  [HttpPut("{id}")] Method
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        // PUT api/<BooksInfoController>/5
        [HttpPut("{id}")]
        public IActionResult PutintoBooks(BookInfoModel bookInfo)
        {
            var putResult=_bookService.PutintoBooksTable(bookInfo);
            if (putResult != null)
            {
                return Ok(putResult);
                
            }
            else
            {
                return NoContent();
            }
            //return _bookService.PutintoBooks(bookInfo).ToList();
        }


        /// <summary>
        /// DeleteBook implements  [HttpDelete("{id}")]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<BooksInfoController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var deletionResult = _bookService.DeleteBookById( id);//change 

            if (deletionResult!=null)
            {
                return Ok($"Book with ID {id} has been deleted.");

            }
            else
            {
                return NotFound($"Book with ID {id} not found.");
            }

        }

    }
}
