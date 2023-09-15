create view [BooksView] as
SELECT Books.id AS book_id, 
       Books.title AS book_title, 
       Author.author_name AS author_name, 
       Publisher.publisher_name AS publisher_name, 
       Books.description AS book_description, 
       BookInfo.language AS book_language, 
       BookInfo.maturityRating AS book_maturityRating, 
       BookInfo.pageCount AS book_pageCount, 
       BookInfo.categories AS book_categories, 
       BookInfo.publishedDate AS book_publishedDate, 
       BookInfo.retailPrice AS book_retailPrice
FROM Books
INNER JOIN Author ON Books.author_id = Author.auth_id
INNER JOIN Publisher ON Books.publisher_id = Publisher.pub_id
INNER JOIN BookInfo ON Books.id = BookInfo.id;


select * from BooksView;