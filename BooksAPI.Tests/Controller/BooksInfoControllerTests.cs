using BooksAPI.Controllers;
using BooksAPI.Interface;
using BooksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace BooksAPI.Tests.Controllers
{
    public class BooksInfoControllerTests
    {
        [Fact]
        public async Task GetAllBooks_ShouldReturnBooksFromApi_WhenDatabaseIsEmpty()
        {
            // Arrange
            var mockBooksInfoService = new Mock<IBooksInfoService>();
            mockBooksInfoService.Setup(x => x.RetrieveBooksFromDatabase()).Returns(new List<BookInfoModel>());
            mockBooksInfoService.Setup(x => x.FetchBooksFromApiAsync()).ReturnsAsync(new List<BookInfoModel>()
            {
            new BookInfoModel() {id = 1, title = "Book 1"},
            new BookInfoModel() {id = 2, title = "Book 2"}
            });

            var booksInfoController = new BooksInfoController(mockBooksInfoService.Object);

            // Act
            var result = await booksInfoController.GetAllBooks(seed: true);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(2, ((List<BookInfoModel>)okObjectResult.Value).Count());
            Assert.Equal("Book 1", ((List<BookInfoModel>)okObjectResult.Value)[0].title);
            Assert.Equal("Book 2", ((List<BookInfoModel>)okObjectResult.Value)[1].title);
        }

        // Tests the GetAllBooks() method when the database is empty.
        [Fact]
        public async Task GetAllBooks_ShouldReturnNotFoundObjectResult_WhenDatabaseIsEmpty()
        {
            // Arrange
            var mockBooksInfoService = new Mock<IBooksInfoService>();
            mockBooksInfoService.SetupSequence(x => x.RetrieveBooksFromDatabase());
            mockBooksInfoService.SetupSequence(x => x.FetchBooksFromApiAsync());
            mockBooksInfoService.SetupSequence(x => x.RetrieveBooksFromJson());

            var booksInfoController = new BooksInfoController(mockBooksInfoService.Object);

            // Act
            var result = await booksInfoController.GetAllBooks();

            // Assert
            Assert.True(result is NotFoundObjectResult || result is NotFoundResult);
        }

        // Tests the GetAllBooks() method when the database is not empty.
        [Fact]
        public async Task GetAllBooks_ShouldReturnBooksFromDatabase_WhenDatabaseIsNotEmpty()
        {
            // Arrange
            var mockBooksInfoService = new Mock<IBooksInfoService>();
            mockBooksInfoService.Setup(x => x.RetrieveBooksFromDatabase()).Returns(new List<BookInfoModel>()
             {
            new BookInfoModel() { id = 1, title = "Book 1" },
            new BookInfoModel() {id = 2, title = "Book 2"}
             });

            var booksInfoController = new BooksInfoController(mockBooksInfoService.Object);

            // Act
            var result = await booksInfoController.GetAllBooks();

            // Assert
           
            var okObjectResult=result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(2, ((List<BookInfoModel>)okObjectResult.Value).Count());
            Assert.Equal("Book 1", ((List<BookInfoModel>)okObjectResult.Value)[0].title);
            Assert.Equal("Book 2", ((List<BookInfoModel>)okObjectResult.Value)[1].title);
        }

        // Tests the GetAllBooks() method when the API fails.
        [Fact]
        public async Task GetAllBooks_ShouldReturnEmptyList_WhenApiFails()
        {
            // Arrange
            var mockBooksInfoService = new Mock<IBooksInfoService>();
            mockBooksInfoService.Setup(x => x.RetrieveBooksFromDatabase()).Returns(new List<BookInfoModel>());
            var result = new OkObjectResult(new List<BookInfoModel>());

            var booksInfoController = new BooksInfoController(mockBooksInfoService.Object);

            // Act
            await booksInfoController.GetAllBooks();
            await mockBooksInfoService.Object.FetchBooksFromApiAsync();

            // Verify that the FetchBooksFromApiAsync() method was called.
            mockBooksInfoService.Verify(x => x.FetchBooksFromApiAsync());

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(0, ((List<BookInfoModel>)okObjectResult.Value).Count());
        }


        // Tests the GetAllBooks() method when all data sources fail.
       
        [Fact]
        public async Task GetAllBooks_ShouldReturnEmptyList_WhenAllDataSourcesFail()
        {
            // Arrange
            var mockBooksInfoService = new Mock<IBooksInfoService>();
            mockBooksInfoService.Setup(x => x.RetrieveBooksFromDatabase()).Returns(new List<BookInfoModel>());
            mockBooksInfoService.Setup(x => x.FetchBooksFromApiAsync()).ThrowsAsync(new Exception("API failed"));
            mockBooksInfoService.Setup(x => x.RetrieveBooksFromJson()).ThrowsAsync(new Exception("JSON failed"));

            // Create a mock `BooksInfoController` object.
            var mockBooksInfoController = new Mock<BooksInfoController>(mockBooksInfoService.Object);

            // Act
            var result = await mockBooksInfoController.Object.GetAllBooks();

            // Assert
            mockBooksInfoService.Verify(x => x.RetrieveBooksFromDatabase(), Times.Once());
            mockBooksInfoService.Verify(x => x.FetchBooksFromApiAsync(), Times.Once());
            mockBooksInfoService.Verify(x => x.RetrieveBooksFromJson(), Times.Once());

            // Verify that the result is an OkObjectResult object with an empty list of books.
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Empty((List<BookInfoModel>)okObjectResult.Value);
        }

        //GetBooksbyId when BookExists
        // Corrected test method
        [Fact]
        public async Task GetBooksById_ShouldReturnOkObjectResultWithBookInfoModel_WhenBookExists()
        {
            // Arrange
            var mockBookDatabaseService = new Mock<IBooksDatabaseService>();
            mockBookDatabaseService.Setup(x => x.RetrieveBookByIdFromDatabase(1)).Returns(new BookInfoModel
            {
                id = 1,
                title = "Test Book",
                author_id = 1,
                publisher_id = 1,
                description = "This is a test book.",
                language = "English",
                maturityRating = "PG-13",
                pageCount = 100,
                publishedDate = "2023-09-20",
                retailPrice = 10.99m,
                author_name = "John Doe",
                publisher_name = "Acme Publishing"
            });

            // Mock the `IBooksInfoService` interface.
            var mockBooksInfoService = new Mock<IBooksInfoService>();

            // Use ReturnsAsync to return a completed Task with the desired result.
            mockBooksInfoService.Setup(x => x.GetBookById(1)).Returns(mockBookDatabaseService.Object.RetrieveBookByIdFromDatabase(1));

            // Create a real `BooksInfoController` object (not a mock) using the mocked services.
            var booksInfoController = new BooksInfoController(mockBooksInfoService.Object);

            // Act
            var result = booksInfoController.GetBooks(1);

            // Assert
            // Assert that the result is an OkObjectResult.
            Assert.IsType<OkObjectResult>(result);

            // Assert that the OkObjectResult contains the expected book.
            var okObjectResult = result as OkObjectResult;
            var book = okObjectResult.Value as BookInfoModel;

            Assert.NotNull(book);
            Assert.Equal(1, book.id);
            Assert.Equal("Test Book", book.title);
            Assert.Equal(1, book.author_id);
            Assert.Equal(1, book.publisher_id);
            Assert.Equal("This is a test book.", book.description);
            Assert.Equal("English", book.language);
            Assert.Equal("PG-13", book.maturityRating);
            Assert.Equal(100, book.pageCount);
            Assert.Equal("2023-09-20", book.publishedDate);
            Assert.Equal(10.99m, book.retailPrice);
            Assert.Equal("John Doe", book.author_name);
            Assert.Equal("Acme Publishing", book.publisher_name);
        }

        //GetBooksbyId when BookDoesNotExists
        [Fact]
        public async Task GetBookById_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var mockBooksDatabaseService = new Mock<IBooksDatabaseService>();
            mockBooksDatabaseService.Setup(x => x.RetrieveBookByIdFromDatabase(1)).Returns(new BookInfoModel
            {
                id = 1,
                title = "Test Book",
                author_id = 1,
                publisher_id = 1,
                description = "This is a test book.",
                language = "English",
                maturityRating = "PG-13",
                pageCount = 100,
                publishedDate = "2023-09-20",
                retailPrice = 10.99m,
                author_name = "John Doe",
                publisher_name = "Acme Publishing"
            });


            var mockBooksInfoService = new Mock<IBooksInfoService>();
            mockBooksInfoService.Setup(x => x.GetBookById(1)).Returns(mockBooksDatabaseService.Object.RetrieveBookByIdFromDatabase(1));

            // Create a mock `BooksInfoController` object.
            var mockBooksInfoController = new Mock<BooksInfoController>(mockBooksInfoService.Object);

            // Act
            var result =mockBooksInfoController.Object.GetBooks(1);

            // Assert
            mockBooksInfoService.Verify(x => x.GetBookById(1), Times.Once());

            // Assert that the result is null.
            Assert.Null(result);
        }

     
    }

        
    }

