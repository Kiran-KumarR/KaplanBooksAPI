
Declare @AllBooksInfo table(   
	book_id INT,
    book_title NVARCHAR(MAX),
    author_name NVARCHAR(MAX),
    publisher_name NVARCHAR(MAX),
    book_description NVARCHAR(MAX),
    book_language NVARCHAR(MAX),
    book_maturityRating NVARCHAR(MAX),
    book_pageCount INT,
    book_categories NVARCHAR(MAX),
    book_publishedDate DATETIME,
    book_retailPrice DECIMAL(10, 2))
--------------
create view [AllBooksInfo] as 
SELECT Books.id AS id, 
       Books.title AS title, 
       Author.author_name AS author_name, 
       Publisher.publisher_name AS publisher_name, 
       Books.description AS description, 
       BookInfo.language AS language, 
       BookInfo.maturityRating AS maturityRating, 
       BookInfo.pageCount AS pageCount, 
       BookInfo.categories AS book_categories, 
       BookInfo.publishedDate AS publishedDate, 
       BookInfo.retailPrice AS retailPrice
FROM Books
INNER JOIN Author ON Books.author_id = Author.auth_id
INNER JOIN Publisher ON Books.publisher_id = Publisher.pub_id
INNER JOIN BookInfo ON Books.id = BookInfo.id;

---------
select * from AllBooksInfo;
