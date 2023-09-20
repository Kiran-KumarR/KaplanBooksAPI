using BooksAPI.Interface;
using BooksAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace BooksAPI.Services
{
    public class BooksDatabaseService : IBooksDatabaseService

    {

        private readonly IConfiguration _configuration;

        public BooksDatabaseService(IConfiguration configuration)
        {

            _configuration = configuration;
        }


        /// <summary>
        /// RetrieveBookByIdFromDatabase used to retrieve rows  from table by passsing id as parameter  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookInfoModel RetrieveBookByIdFromDatabase(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectSql = " select B.* ,A.author_name,P.publisher_name from Books B inner join  Author A  ON  B.author_id = A.auth_id inner join Publisher P on B.publisher_id = P.pub_id where B.id=@id";
                    SqlCommand command = new SqlCommand(selectSql, connection);
                    command.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var book = new BookInfoModel
                        {
                            id = reader.GetInt32(0),
                            title = reader.GetString(1),
                            author_id = reader.GetInt32(2),
                            publisher_id = reader.GetInt32(3),
                            description = reader.GetString(4),
                            language = reader.GetString(5),
                            maturityRating = reader.GetString(6),
                            pageCount = reader.GetInt32(7),
                            publishedDate = reader.GetString(8),
                            retailPrice = reader.GetDecimal(9),

                            author_name = reader.GetString(10),
                            publisher_name = reader.GetString(11)


                        };

                        return book;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return null;
        }


        /// <summary>
        /// DeleteBookFromDatabase used for deleting a record by passing id as parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BookInfoModel DeleteBookFromDatabase(int id)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve book information before deletion
                    string selectSql = "SELECT * FROM Books WHERE id = @id";
                    SqlCommand selectCommand = new SqlCommand(selectSql, connection);
                    selectCommand.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve book information here
                            int bookId = reader.GetInt32(0);
                            string bookTitle = reader.GetString(1);
                            int authorId = reader.GetInt32(2);
                            int publisherId = reader.GetInt32(3);
                            string description = reader.GetString(4);
                            string language = reader.GetString(5);
                            string maturityRating = reader.GetString(6);
                            int pageCount = reader.GetInt32(7);
                            string publishedDate = reader.GetString(8);
                            decimal retailPrice = reader.GetDecimal(9);

                            
                            BookInfoModel deletedBookInfo = new BookInfoModel
                            {
                                id = bookId,
                                title = bookTitle,
                                author_id = authorId,
                                publisher_id = publisherId,
                                description = description,
                                language = language,
                                maturityRating = maturityRating,
                                pageCount = pageCount,
                                publishedDate = publishedDate,
                                retailPrice = retailPrice
                                
                            };

                            // Close the SqlDataReader before executing the deletion command
                            reader.Close();

                            // Perform the actual book deletion
                            string deleteSql = "DELETE FROM Books WHERE id = @id";
                            SqlCommand deleteCommand = new SqlCommand(deleteSql, connection);
                            deleteCommand.Parameters.AddWithValue("@id", id);

                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            // If rowsAffected is greater than 0, the book was deleted successfully
                            if (rowsAffected > 0)
                            {
                                return deletedBookInfo; // Return book info after successful deletion
                            }
                        }
                    }

                }

                return null; // Return null if the book was not found or not deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null; 
            }
        }
        

        /// <summary>
        /// PutIntoBooks method is used to update the existing records in the table
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        public BookInfoModel PutIntoBooks(int book_id, BookInfoModel bookInfo)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            List<BookInfoModel> list = new List<BookInfoModel>();
            
            SqlCommand checkBookCommand = new SqlCommand("SELECT COUNT(*) FROM Books WHERE id = @Id;", sqlConnection);
            checkBookCommand.Parameters.AddWithValue("@Id", book_id);
            int bookCount = (int)checkBookCommand.ExecuteScalar();
            if (bookCount == 0)
            {
                Console.WriteLine($"Book ID {book_id}  not found");
                return new BookInfoModel { id = book_id, message = $" Book ID {book_id} not found " };
            }

            int authId = GetOrCreateAuthorId(sqlConnection, bookInfo.author_name);
            int pubId = GetOrCreatePublisherId(sqlConnection, bookInfo.publisher_name);

            SqlCommand sqlCommand = new SqlCommand("UPDATE Books SET " +
        "title = ISNULL(@Title, title), " +
        "author_id = ISNULL(@Author_ID, author_id), " +
        "publisher_id = ISNULL(@Publisher_ID, publisher_id), " +
        "description = ISNULL(@Description, description), " +
        "language = ISNULL(@Language, language), " +
        "maturityRating = ISNULL(@MaturityRating, maturityRating), " +
        "pageCount = ISNULL(@PageCount, pageCount), " +
        "publishedDate = ISNULL(@PublishedDate, publishedDate), " +
        "retailPrice = ISNULL(@RetailPrice, retailPrice) " +
        "WHERE id = @Id;", sqlConnection);  //SELECT * FROM Author WHERE auth_id = @id

            sqlCommand.Parameters.AddWithValue("@Id", book_id);
            sqlCommand.Parameters.AddWithValue("@Title", bookInfo.title);
            sqlCommand.Parameters.AddWithValue("@Author_ID", authId);
            sqlCommand.Parameters.AddWithValue("@Publisher_ID", pubId);
            sqlCommand.Parameters.AddWithValue("@Description", bookInfo.description);

            sqlCommand.Parameters.AddWithValue("@Language", bookInfo.language);
            sqlCommand.Parameters.AddWithValue("@MaturityRating", bookInfo.maturityRating);


            sqlCommand.Parameters.AddWithValue("@PageCount", bookInfo.pageCount);
            sqlCommand.Parameters.AddWithValue("@PublishedDate", bookInfo.publishedDate);
            sqlCommand.Parameters.AddWithValue("@RetailPrice", bookInfo.retailPrice);
          

            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            var book = new BookInfoModel
            {
                id = book_id,
                title = bookInfo.title,
                auth_id = authId,
                pub_id = pubId,
                description = bookInfo.description,
                language = bookInfo.language,
                maturityRating = bookInfo.maturityRating,
                publishedDate = bookInfo.publishedDate,
                retailPrice = bookInfo.retailPrice,
                author_name=bookInfo.author_name,
                publisher_name=bookInfo.publisher_name
            }; return book;
        }

        /// <summary>
        /// PostIntoBooks method used to Insert records into the table
        /// </summary>
        /// <param name="bookInfo"></param>
        /// <returns></returns>
        public BookInfoModel PostIntoBooks(BookInfoModel bookInfo)
        {


            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //List<BookInfoModel> list = new List<BookInfoModel>();
            sqlConnection.Open();

            int bookId = GetOrCreateBookId(sqlConnection);
            int authId = GetOrCreateAuthorId(sqlConnection, bookInfo.author_name);
            int pubId = GetOrCreatePublisherId(sqlConnection, bookInfo.publisher_name);

            SqlCommand insertBookCommand = new SqlCommand("INSERT INTO Books(id, title, author_id, publisher_id, description, language, maturityRating, pageCount, publishedDate, retailPrice) " +
                "VALUES (@BookId, @Title, @Author_ID, @Publisher_ID, @Description, @Language, @MaturityRating, @PageCount, @PublishedDate, @RetailPrice);", sqlConnection);

            insertBookCommand.Parameters.AddWithValue("@BookId", bookId);
            insertBookCommand.Parameters.AddWithValue("@Title", bookInfo.title);
            insertBookCommand.Parameters.AddWithValue("@Author_ID", authId);
            insertBookCommand.Parameters.AddWithValue("@Publisher_ID", pubId);
            insertBookCommand.Parameters.AddWithValue("@Description", bookInfo.description);
            insertBookCommand.Parameters.AddWithValue("@Language", bookInfo.language);
            insertBookCommand.Parameters.AddWithValue("@MaturityRating", bookInfo.maturityRating);
            insertBookCommand.Parameters.AddWithValue("@PageCount", bookInfo.pageCount);
            insertBookCommand.Parameters.AddWithValue("@PublishedDate", bookInfo.publishedDate);
            insertBookCommand.Parameters.AddWithValue("@RetailPrice", bookInfo.retailPrice);


            insertBookCommand.ExecuteNonQuery();
            sqlConnection.Close();

            // Since you just inserted a book, you can directly add it to the list
            var list= new BookInfoModel
            {
                id = bookId,
                title = bookInfo.title,
                auth_id = authId,
                pub_id = pubId,
                description = bookInfo.description,
                language = bookInfo.language,
                maturityRating = bookInfo.maturityRating,
                publishedDate = bookInfo.publishedDate,
                retailPrice = bookInfo.retailPrice
            };

            return list;


        }


        /// <summary>
        /// List<BookInfoModel> RetrieveBooksFromDatabase()
        /// This method returns  the list of records stored from Database
        /// </summary>
        /// <returns></returns>
        public List<BookInfoModel> RetrieveBooksFromDatabase()
        {
            List<BookInfoModel> books = new List<BookInfoModel>();

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectSql = " select B.* ,A.author_name,P.publisher_name from Books B inner join  Author A  ON  B.author_id = A.auth_id inner join Publisher P on B.publisher_id = P.pub_id ";
                    SqlCommand command = new SqlCommand(selectSql, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var book = new BookInfoModel
                            {
                                id = reader.GetInt32(0),
                                title = reader.GetString(1),
                                author_id = reader.GetInt32(2),
                                publisher_id = reader.GetInt32(3),
                                description = reader.GetString(4),
                                language = reader.GetString(5),
                                maturityRating = reader.GetString(6),
                                pageCount = reader.GetInt32(7),
                                publishedDate = reader.GetString(8),
                                retailPrice = reader.GetDecimal(9),

                                author_name = reader.GetString(10),
                                publisher_name = reader.GetString(11)

                            };

                            books.Add(book);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return books;
        }


        /// <summary>
        ///   Task<List<BookInfoModel>> FetchBooksFromApiAsync()
        ///   This method returns an List<BookInfoModel> from fetching from the API call
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookInfoModel>> FetchBooksFromApiAsync()
        {
            var httpClient = new HttpClient();
            //var apiUrl = "https://www.googleapis.com/books/v1/volumes?q=kaplan%20test%20prep";
            var apiUrl = "https://www.bing.com/books/v1/volumes?q=kaplan%20test%20prep";


            try
            {
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<GoogleBooksApiResponse>(content);

                    if (responseObject.items != null)
                    {
                        var bookInfos = new List<BookInfoModel>();

                        foreach (var item in responseObject.items)
                        {
                            bookInfos.Add(new BookInfoModel
                            {
                                //Id = item.id,
                                title = item.volumeInfo.title,
                                author_name = item.volumeInfo.authors != null ? string.Join(", ", item.volumeInfo.authors) : "No author",
                                publisher_name = item.volumeInfo.publisher,
                                description = item.volumeInfo.description,
                                language = item.volumeInfo.language,
                                maturityRating = item.volumeInfo.maturityRating,
                                pageCount = item.volumeInfo.pageCount,
                                // categories = item.volumeInfo.categories,
                                publishedDate = item.volumeInfo.publishedDate,
                                retailPrice = item.volumeInfo.retailPrice,

                            });
                        }

                        return bookInfos;
                    }
                }
                else
                {
                    Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                  
                    await RetrieveBooksFromJson();
                    // check comment
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API request failed with exception: {ex.Message}");
            }

            return new List<BookInfoModel>();
        }


        /// <summary>
        /// Task<List<BookInfoModel>> RetrieveBooksFromJson()
        /// This method returns an List<BookInfoModel> from fetching the data from jsonFilePath 
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        public async Task<List<BookInfoModel>> RetrieveBooksFromJson()
        {
            try
            {
                var jsonFilePath = "C:\\Users\\KKumarR\\Desktop\\KaplanBooksAPI\\BooksAPI\\Database\\kaplan_book.json";
                using (StreamReader reader = new StreamReader(jsonFilePath))
                {
                    var jsonString = reader.ReadToEnd();
                    var bookInfos = JsonConvert.DeserializeObject<List<GoogleBooksApiResponse>>(jsonString);
                    var bookInfo = new List<BookInfoModel>();

                    foreach (var item in bookInfos[0].items)
                    {
                        bookInfo.Add(new BookInfoModel
                        {
                            //Id = item.id,
                            title = item.volumeInfo.title,
                            author_name = item.volumeInfo.authors != null ? string.Join(", ", item.volumeInfo.authors) : "No author",
                            publisher_name = item.volumeInfo.publisher,
                            description = item.volumeInfo.description,
                            language = item.volumeInfo.language,
                            maturityRating = item.volumeInfo.maturityRating,
                            pageCount = item.volumeInfo.pageCount,
                            // categories = item.volumeInfo.categories,
                            publishedDate = item.volumeInfo.publishedDate,
                            retailPrice = item.volumeInfo.retailPrice,
                        });
                    }
                    return bookInfo;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RetrieveBooksFromJson: {ex.Message}");
            }

            return new List<BookInfoModel>();
        }


        /// <summary>
        /// StoreBooksInDatabase() methods takes List<BookInfoModel> as parameter
        /// This method is used for storing the result of API or jsonPathFile to Database
        /// </summary>
        /// <param name="bookInfos"></param>
        /// <returns></returns>
        public async Task StoreBooksInDatabase(List<BookInfoModel> bookInfos)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();

                foreach (var bookInfo in bookInfos)
                {
                    int auth_id = GetOrCreateAuthorId(sqlConnection, bookInfo.author_name);
                    int pub_id = GetOrCreatePublisherId(sqlConnection, bookInfo.publisher_name);
                    int bookId = GetOrCreateBookId(sqlConnection);

                    string insertBookSql = "INSERT INTO Books (id, title, author_id, publisher_id, description,language,maturityRating,pageCount,publishedDate,retailPrice) VALUES (@BookId, @Title, @AuthorId, @PublisherId, LEFT(@Description, 1000),@language,@maturityRating,@pageCount,@publishedDate,@retailPrice)";
                    SqlCommand insertBookCommand = new SqlCommand(insertBookSql, sqlConnection);
                    insertBookCommand.Parameters.AddWithValue("@BookId", bookId);
                    insertBookCommand.Parameters.AddWithValue("@Title", bookInfo.title);
                    insertBookCommand.Parameters.AddWithValue("@AuthorId", auth_id);
                    insertBookCommand.Parameters.AddWithValue("@PublisherId", pub_id);
                    insertBookCommand.Parameters.AddWithValue("@Description", bookInfo.description);
                    insertBookCommand.Parameters.AddWithValue("@language", bookInfo.language);
                    insertBookCommand.Parameters.AddWithValue("@maturityRating", bookInfo.maturityRating);
                    insertBookCommand.Parameters.AddWithValue("@pageCount", bookInfo.pageCount);
                    insertBookCommand.Parameters.AddWithValue("@publishedDate", bookInfo.publishedDate);
                    insertBookCommand.Parameters.AddWithValue("@retailPrice", bookInfo.retailPrice);

                    insertBookCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StoreBooksInDatabase: {ex.Message}");
                throw; // Re-throw the exception to handle it at a higher level.
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close(); // Ensure the connection is closed, even in case of exceptions.
                }
            }
        }


        /// <summary>
        /// GetOrCreateBookId() returns 1 if there are no records else return the maximum  of record incremented by 1
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int GetOrCreateBookId(SqlConnection connection)
        {
            string selectMaxBookIdSql = "SELECT MAX(id) FROM Books";
            SqlCommand selectMaxBookIdCommand = new SqlCommand(selectMaxBookIdSql, connection);
            //connection.Open();
            var maxId = selectMaxBookIdCommand.ExecuteScalar();
            if (maxId == DBNull.Value)
            {
                return 1;
            }
            else
            {
                return (int)maxId + 1;
            }
        }


        /// <summary>
        /// GetOrCreatePublisherId() 
        /// This methods return the PublisherId if there exists some records
        /// Else  returns the PublisherId i.e, set as IDENTITY Column in Table
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="publisher_name"></param>
        /// <returns></returns>
        public int GetOrCreatePublisherId(SqlConnection connection, string publisher_name)
        {

            string selectPublisherSql = "SELECT pub_id FROM Publisher WHERE publisher_name = @PublisherName";
            SqlCommand publisherCommand = new SqlCommand(selectPublisherSql, connection);
            publisherCommand.Parameters.AddWithValue("@PublisherName", publisher_name);


            object publisherIdResult = publisherCommand.ExecuteScalar();

            if (publisherIdResult != null)
            {
                return (int)publisherIdResult;
            }
            else
            {

                string insertPublisherSql = "INSERT INTO Publisher (publisher_name) VALUES (@PublisherName); SELECT SCOPE_IDENTITY();";
                SqlCommand insertPublisherCommand = new SqlCommand(insertPublisherSql, connection);
                insertPublisherCommand.Parameters.AddWithValue("@PublisherName", publisher_name);

                return Convert.ToInt32(insertPublisherCommand.ExecuteScalar());
            }
        }


        /// <summary>
        /// GetOrCreateAuthorId() 
        /// This methods return the AuthorId if there exists some records
        /// Else  returns the AuthorId i.e ,set as IDENTITY column in Table
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="author_name"></param>
        /// <returns></returns>
        public int GetOrCreateAuthorId(SqlConnection connection, string author_name)
        {
            string selectAuthorSql = "SELECT auth_id FROM Author WHERE author_name = @AuthorName";
            SqlCommand authorCommand = new SqlCommand(selectAuthorSql, connection);
            authorCommand.Parameters.AddWithValue("@AuthorName", author_name);

            object authorIdResult = authorCommand.ExecuteScalar();

            if (authorIdResult != null)
            {
                return (int)authorIdResult;
            }
            else
            {

                string insertAuthorSql = "INSERT INTO Author (author_name) VALUES (@AuthorName); SELECT SCOPE_IDENTITY();";
                SqlCommand insertAuthorCommand = new SqlCommand(insertAuthorSql, connection);
                insertAuthorCommand.Parameters.AddWithValue("@AuthorName", author_name);

                return Convert.ToInt32(insertAuthorCommand.ExecuteScalar());
            }
        }



        /// <summary>
        /// GetBookId () returns 1 if there are no records else return the maximum  of records
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int GetBookId(SqlConnection connection)
        {
            string selectMaxBookIdSql = "SELECT MAX(id) FROM Books";
            SqlCommand selectMaxBookIdCommand = new SqlCommand(selectMaxBookIdSql, connection);
            connection.Open();
            var maxId = selectMaxBookIdCommand.ExecuteScalar();
            if (maxId == DBNull.Value)
            {
                return 1;
            }
            else
            {
                return (int)maxId;
            }

        }


     

    }
}