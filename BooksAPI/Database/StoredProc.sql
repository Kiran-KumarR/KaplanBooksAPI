/*


*/

-- Create a stored procedure to insert data into Books and BookInfo
CREATE PROCEDURE InsertIntoAllBooksInfo
    @id INT,
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
    -- first insert data into the Books table absed on auhtor and publsher table 
    INSERT INTO Books (id, title, author_id, publisher_id, description)
    SELECT
        @id,
        @title,
        A.auth_id,
        P.pub_id,
        @description
    FROM Author A INNER JOIN Publisher P ON P.publisher_name = @publisher_name
    WHERE A.author_name = @author_name;

    -- Insert data into the BookInfo table
    INSERT INTO BookInfo (id, language, maturityRating, pageCount, categories, publishedDate, retailPrice)
    SELECT
        @id,
        @language,
        @maturityRating,
        @pageCount,
        @categories,
        @publishedDate,
        @retailPrice;
END;

----insert the vlaue via procedure
EXEC InsertIntoAllBooksInfo
    @id = 10,
    @title = 'Fire Wings',
    @author_name = 'Kiran Kumar R',
    @publisher_name = 'Wagon',
    @description = 'Description of the sample book by kiran',
    @language = 'English',
    @maturityRating = 'Mature',
    @pageCount = 200,
    @categories = 'Biography',
    @publishedDate = '2023-01-11',
    @retailPrice = 89.99;


--to check if its inserted all those table 

select * from BookInfo;
select * from Author;
select * from Publisher;
select * from Books;


--insted run the stored proc to check EXEC GetAllBooksInfo;

EXEC GetAllBooksInfo;


SELECT * FROM Books WHERE id = 10;
SELECT * FROM BookInfo WHERE id = 10;
SELECT * FROM Author WHERE author_name = 'Kiran Kumar R';


---this join is wokring fine and ..



SELECT A.auth_id, P.pub_id
FROM Author A
INNER JOIN Publisher P ON P.publisher_name = 'Penguin Books'
WHERE A.author_name = 'Kiran';
