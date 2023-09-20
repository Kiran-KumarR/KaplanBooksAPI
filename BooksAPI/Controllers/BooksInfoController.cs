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

        private readonly IBooksInfoService _bookService;
      

        public BooksInfoController( IBooksInfoService bookService)
        {
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
            try
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
            catch (Exception ex)
            {
                return NotFound();
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
                
                    var booksFromJson = await _bookService.RetrieveBooksFromJson();

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
                return Ok($"Insert is sucessfull for Book_ID {postResult.id} ");
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// PutintoBooks implements  [HttpPut("{id}")] Method
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        // PUT api/<BooksInfoController>/5
        [HttpPut("{book_id}")]
        public IActionResult PutintoBooks(int book_id,BookInfoModel bookInfo)
        {
            var putResult=_bookService.PutintoBooksTable(book_id,bookInfo);
           
            if (putResult != null)
            {
                return Ok(putResult);
                
            }
            else
            {
                return NotFound($"Book with ID {book_id} not found.");
            }
            
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
