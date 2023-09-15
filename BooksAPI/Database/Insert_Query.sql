select * from Author;


select * from Publisher;


select * from Books;

select * from Author;

select * from Publisher;

insert into Books(id,title,author_id,publisher_id,description,language,maturityRating,pageCount,publishedDate,retailPrice) 
values (1,'HarryPotter',3000,2000,'Fiction Book into the world of harry potter','English','Mature',420,'12-09-2008',88.31);

SELECT B.id , B.title,A.auth_id ,   A.author_name ,  P.pub_id  ,  P.publisher_name ,   B.description  ,  B.language , B.maturityRating ,  B.pageCount,    B.publishedDate, B.retailPrice FROM    Books B INNER JOIN Author A ON B.author_id = A.auth_id INNER JOIN Publisher P ON B.publisher_id = P.pub_id ;