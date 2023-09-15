/*
GetAllBooksInfoById [http get{id} ]method

*/
CREATE PROCEDURE GetAllBooksInfoById
    @id INT
AS
BEGIN
    SELECT
        B.id ,B.title ,
        A.author_name,
        P.publisher_name,
        B.description ,
        BI.language,
        BI.maturityRating ,
        BI.pageCount ,
        BI.categories ,
        BI.publishedDate ,
        BI.retailPrice
    FROM
        Books AS B
    INNER JOIN
        Author AS A ON B.author_id = A.auth_id
    INNER JOIN
        Publisher AS P ON B.publisher_id = P.pub_id
    INNER JOIN
        BookInfo AS BI ON B.id = BI.id
    WHERE
        B.id = @id;
END;


--to run 

exec GetAllBooksInfoById @id='1';


