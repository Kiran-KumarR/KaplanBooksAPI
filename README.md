# KaplanBooksAPI

## Summary

- KaplanBooksAPI is a versatile tool designed for interfacing with APIs and storing the obtained data in a SQL Server Database. It supports a range of HTTP methods, including GET, POST, PUT, and DELETE, and provides robust functionality for handling various scenarios.

### Packages Required

- Install-Package Newtonsoft.Json Version="13.0.3"  //used for parsing json file
- Install-Package System.Data.SqlClient Version=4.8.5   //used to establish connection from your SQL Server (LocalDB)
- Install-Package Microsoft.AspNetCore.OpenApi Version=7.0.10   //used to run your application as WEB API
- Install-Package Moq Version=4.20.69   //used to create a mock for your original function
- Install-Package xunit Version=2.5.1   //used to create unit test

### Tools and Technology

- Visual Studio 2022
- Microsoft SQL Server Management Studio
- .Net Core 6.0 or above
- Postman 
- C#  Basics and OOPS Concepts


##  Development
### It performs Various the HTTP Request:

- GET ALL BOOKS : api/BooksInfo?seed=true
- GET BOOKS-BY-ID : /api/BooksInfo/{bookId}
- POST : api/BooksInfo
- PUT BY BOOKSID : /api/BooksInfo/{book_id}
- DELETE BOOKS-BY-ID : /api/BooksInfo/{id}

### UNIT TEST CASES
- Unit testing is a software development process in which the smallest testable parts of an application, called units, are individually scrutinized for proper operation. It performs all the unit test cases for GET ALL BOOKS api and GET BOOKS-BY-ID using XUnit:

- GetAllBooks_ShouldReturnBooksFromApi_WhenDatabaseIsEmpty
- GetAllBooks_ShouldReturnNotFoundObjectResult_WhenDatabaseIsEmpty
- GetAllBooks_ShouldReturnBooksFromDatabase_WhenDatabaseIsNotEmpty
- GetAllBooks_ShouldReturnEmptyList_WhenApiFails
- GetAllBooks_ShouldReturnEmptyList_WhenAllDataSourcesFail
- GetBooksById_ShouldReturnOkObjectResultWithBookInfoModel_WhenBookExists
- GetBookById_ShouldReturnNull_WhenBookDoesNotExist 


### Run

- Clone the Repo
- Set up the Database Design as shown in Database folder include some key constraints(Primary-Foreign Key Relations)
- Try to Debug by adding breakpoints(Fn+F9 Key) for understanding control flow
- Clean,Re-Build,Build
-  Hit Run
  
