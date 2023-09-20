using BooksAPI.Interface;
using BooksAPI.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Services
{
    public class BooksInfoService:IBooksInfoService
      
    {
       
        private readonly IBooksDatabaseService _databaseService;
      
        /// <summary>
        /// BooksInfoService is an constructor of BooksInfoSevice 
        /// Dependency Injection
        /// </summary>
        /// <param name="databaseService"></param>
        public BooksInfoService( IBooksDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        /// <summary>
        /// GetBookById(int id) returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookInfoModel GetBookById(int id)
        {
            return _databaseService.RetrieveBookByIdFromDatabase(id);
        }


        /// <summary>
        /// GetBookId method returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int GetBookId(SqlConnection  connection)
        {
            return _databaseService.GetBookId(connection);
        }


        /// <summary>
        /// GetOrCreatePublisherId returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="publisher_name"></param>
        /// <returns></returns>
        public int  GetOrCreatePublisherId(SqlConnection connection, string publisher_name)
        {

            return _databaseService.GetOrCreatePublisherId(connection, publisher_name);
        }


        /// <summary>
        /// GetOrCreateAuthorId  returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="author_name"></param>
        /// <returns></returns>
        public int GetOrCreateAuthorId(SqlConnection connection, string author_name)
        {
            return _databaseService.GetOrCreateAuthorId(connection, author_name);
        }


        /// <summary>
        /// GetOrCreateBookId returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int GetOrCreateBookId(SqlConnection connection)
        {
            return _databaseService.GetOrCreateBookId(connection);
        }


        /// <summary>
        /// DeleteBookById  returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookInfoModel DeleteBookById(int id)
        {
            return _databaseService.DeleteBookFromDatabase(id);
        }


        /// <summary>
        /// PutintoBooksTable returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        public BookInfoModel PutintoBooksTable(int book_id, BookInfoModel bookInfo)
        {
            return _databaseService.PutIntoBooks(book_id,bookInfo);
        }


        /// <summary>
        /// PostintoBooksTable returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        public BookInfoModel PostintoBooksTable(BookInfoModel bookInfo)
        {
            return _databaseService.PostIntoBooks(bookInfo);
        }


        /// <summary>
        /// FetchBooksFromApiAsync returns an method of IBooksDatabaseService
        /// </summary>
        /// <returns></returns>
        public Task<List<BookInfoModel>> FetchBooksFromApiAsync()
        {
            return  _databaseService.FetchBooksFromApiAsync();

        }


        /// <summary>
        /// RetrieveBooksFromDatabase returns an method of IBooksDatabaseService
        /// </summary>
        /// <returns></returns>
        public List<BookInfoModel> RetrieveBooksFromDatabase()
        {
            return _databaseService.RetrieveBooksFromDatabase();
        }


        /// <summary>
        /// RetrieveBooksFromJson returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        public Task<List<BookInfoModel>> RetrieveBooksFromJson()
        {
            return _databaseService.RetrieveBooksFromJson();
        }


        /// <summary>
        /// StoreBooksInDatabase returns an method of IBooksDatabaseService
        /// </summary>
        /// <param name="bookInfos"></param>
        /// <returns></returns>
        public Task StoreBooksInDatabase(List<BookInfoModel> bookInfos)
        {
            return _databaseService.StoreBooksInDatabase(bookInfos);
        }
       
    }
}
