select * from AllBooksInfo;
/*
A trigger cannot directly execute a SELECT statement to retrieve data from multiple tables using a join and return the output. 
use stored Proc
now in contoller create GetAllBooksInfo function  //[http get method]
*/
CREATE PROCEDURE GetAllBooksInfo
AS
BEGIN
    SELECT
        B.id AS id,
        B.title AS title,
        A.author_name AS auth_name,
        P.publisher_name AS publisher_name,
        B.description AS description,
        BI.language AS language,
        BI.maturityRating AS maturityRating,
        BI.pageCount AS pageCount,
        BI.categories AS categories,
        BI.publishedDate AS publishedDate,
        BI.retailPrice AS retailPrice
    FROM
        Books AS B
    INNER JOIN
        Author AS A ON B.author_id = A.auth_id
    INNER JOIN
        Publisher AS P ON B.publisher_id = P.pub_id
    INNER JOIN
        BookInfo AS BI ON B.id = BI.id;
END;

---to call stored proc

EXEC GetAllBooksInfo;

--DROP PROCEDURE GetAllBooksInfo;