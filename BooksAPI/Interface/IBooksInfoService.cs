using BooksAPI.Models;
using System.Data.SqlClient;

namespace BooksAPI.Interface
{
    /// <summary>
    /// IBooksInfoService contains all the Interfaces that are implemented in BooksInfoService
    /// </summary>
    public interface IBooksInfoService
    {
        Task<List<BookInfoModel>> FetchBooksFromApiAsync();
        Task<List<BookInfoModel>> RetrieveBooksFromJson();

        List<BookInfoModel> RetrieveBooksFromDatabase();
        Task StoreBooksInDatabase(List<BookInfoModel> bookInfos);
        int GetOrCreatePublisherId(SqlConnection connection, string publisher_name);
        int GetOrCreateBookId(SqlConnection connection);

        BookInfoModel GetBookById(int id);

        BookInfoModel DeleteBookById(int id);


        BookInfoModel PutintoBooksTable(int book_id,BookInfoModel bookInfo);

        BookInfoModel PostintoBooksTable(BookInfoModel bookInfo);

  
    }
}
