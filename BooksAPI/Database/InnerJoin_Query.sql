select * from Books;

select * from Author;

select * from Publisher;


--
               /*string selectSql = "SELECT b.book_id, b.title, a.author_name, p.publisher_name, p.published_date, b.description" +
                                       " FROM Books b" +
                                       " INNER JOIN Authors a ON b.author_id = a.author_id" +
                                       " INNER JOIN Publishers p ON b.publisher_id = p.publisher_id" +
                                       " WHERE b.book_id = @BookId"*/

select B.* ,A.author_name,P.publisher_name from Books B inner join  Author A  ON  B.author_id = A.auth_id inner join Publisher P on B.publisher_id = P.pub_id 

select B.* ,A.author_name,P.publisher_name from Books B inner join  Author A  ON  B.author_id = A.auth_id inner join Publisher P on B.publisher_id = P.pub_id where B.id='2';


--this below also works
select B.* ,Author.author_name,Publisher.publisher_name from Books B inner join  Author   ON  B.author_id = Author.auth_id inner join Publisher  on B.publisher_id = Publisher.pub_id where B.id='1';

DELETE FROM Books WHERE id = '10';