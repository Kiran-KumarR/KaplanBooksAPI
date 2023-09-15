SELECT
    B.id ,
    B.title,
    A.auth_id ,
    A.author_name ,
    P.pub_id  ,
    P.publisher_name ,
    B.description  ,
    BI.language ,
    BI.maturityRating ,
    BI.pageCount,
    BI.categories ,
    BI.publishedDate,
    BI.retailPrice 
FROM
    Books B
INNER JOIN
    Author A ON B.author_id = A.auth_id
INNER JOIN
    Publisher P ON B.publisher_id = P.pub_id
INNER JOIN
    BookInfo BI ON B.id = BI.id;


