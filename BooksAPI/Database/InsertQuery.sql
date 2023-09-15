INSERT INTO Author (author_name) VALUES
('J.K. Rowling'),
('George Orwell'),
('Jane Austen');

INSERT INTO Publisher (publisher_name) VALUES
('Bloomsbury'),
('Penguin Books'),
('HarperCollins');

----------
select * from Author;

select * from Publisher;

select * from Books;

select * from BookInfo;

--drop table BookInfo;
--drop table Books;
--drop table Publisher;


-----------
INSERT INTO Books (id, title, author_id, publisher_id, description) VALUES
(1, 'Harry Potter and the Philosopher''s Stone', 3000, 2000, 'The first book in the Harry Potter series.'),
(2, '1984', 3001, 2001, 'A dystopian novel by George Orwell.'),
(3, 'Pride and Prejudice', 3002, 2002, 'A classic novel by Jane Austen.');


INSERT INTO BookInfo (id, language, maturityRating, pageCount, categories, publishedDate, retailPrice) VALUES
(1, 'English', 'Mature', 320, 'Fantasy', '1997-06-26', 19.99),
(2, 'English', 'Mature', 328, 'Dystopian', '1949-06-08', 15.99),
(3, 'English', 'Mature', 432, 'Romance', '1813-01-28', 14.99);

---------------
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
