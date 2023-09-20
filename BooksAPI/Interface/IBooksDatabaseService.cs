using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BooksAPI.Interface
{
    /// <summary>
    /// IBooksDatabaseService contains all the Interfaces that are implemented in BooksDatabseService
    /// </summary>
    public interface IBooksDatabaseService
    {
       BookInfoModel RetrieveBookByIdFromDatabase(int id);

       BookInfoModel DeleteBookFromDatabase(int id);

        List<BookInfoModel> RetrieveBooksFromDatabase();

        Task<List<BookInfoModel>> FetchBooksFromApiAsync();

      
        BookInfoModel PutIntoBooks(int book_id, BookInfoModel bookInfo);

        BookInfoModel PostIntoBooks(BookInfoModel bookInfo);
        int GetBookId(SqlConnection connection);

        int GetOrCreatePublisherId(SqlConnection connection, string publisher_name);

        int GetOrCreateAuthorId(SqlConnection connection, string author_name);

        int GetOrCreateBookId(SqlConnection connection);

        Task StoreBooksInDatabase(List<BookInfoModel> bookInfos);

        Task<List<BookInfoModel>> RetrieveBooksFromJson();

        SqlConnection CreateSqlConnection();
    }
}
