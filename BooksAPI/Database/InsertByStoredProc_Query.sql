
---figure this out not wokrking


CREATE PROCEDURE InsertIntoBooks
    @title VARCHAR(50),
    @author_name VARCHAR(50),
    @publisher_name VARCHAR(50),
    @description VARCHAR(1000),
    @language VARCHAR(20),
    @maturityRating VARCHAR(30),
    @pageCount INT,
    @categories VARCHAR(50),
    @publishedDate VARCHAR(50),
    @retailPrice DECIMAL(6, 3)
AS
BEGIN
    DECLARE @author_id INT;
    DECLARE @publisher_id INT;


    IF NOT EXISTS (SELECT 1 FROM Author WHERE author_name = @author_name)
    BEGIN
        INSERT INTO Author (author_name) VALUES (@author_name);
    END

    
    IF NOT EXISTS (SELECT 1 FROM Publisher WHERE publisher_name = @publisher_name)
    BEGIN
        INSERT INTO Publisher (publisher_name) VALUES (@publisher_name);
    END

    
    SELECT @author_id = auth_id FROM Author WHERE author_name = @author_name;
    SELECT @publisher_id = pub_id FROM Publisher WHERE publisher_name = @publisher_name;

   
    INSERT INTO Books (id, title, author_id, publisher_id, description)
    VALUES (NEXT VALUE FOR dbo.Books.id, @title, @author_id, @publisher_id, @description);

    -- get book_id
    DECLARE @book_id INT;
    SET @book_id = SCOPE_IDENTITY();

    -- Insert into the BookInfo table
    INSERT INTO BookInfo (id, language, maturityRating, pageCount, categories, publishedDate, retailPrice)
    VALUES (@book_id, @language, @maturityRating, @pageCount, @categories, @publishedDate, @retailPrice);
END;


EXEC InsertIntoBooks
    @title = 'Born King',
    @author_name = 'Kiran R',
    @publisher_name = 'Wagoner',
    @description = 'Description of the sample book by Kiran Kumar',
    @language = 'English',
    @maturityRating = 'Mature',
    @pageCount = 200,
    @categories = 'Biography',
    @publishedDate = '2023-01-11',
    @retailPrice = 89.99;


--
SELECT * FROM Author WHERE author_name = 'Kiran R';

SELECT * FROM Books WHERE id = '2';
SELECT * FROM BookInfo WHERE id ='2' ;

select * from BookInfo;

select * from Author;

select * from Publisher;

select * from Books;


---------
SELECT Books.id AS id, 
       Books.title AS title, 
       Author.author_name AS author_name, 
       Publisher.publisher_name AS publisher_name, 
       Books.description AS description, 
       BookInfo.language AS language, 
       BookInfo.maturityRating AS maturityRating, 
       BookInfo.pageCount AS pageCount, 
       BookInfo.categories AS categories, 
       BookInfo.publishedDate AS publishedDate, 
       BookInfo.retailPrice AS retailPrice
FROM Books
INNER JOIN Author ON Books.author_id = Author.auth_id
INNER JOIN Publisher ON Books.publisher_id = Publisher.pub_id
INNER JOIN BookInfo ON Books.id = BookInfo.id;

