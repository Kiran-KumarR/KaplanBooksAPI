select * from Books;
select * from Author;
select * from Publisher;
insert into Books(id,title,author_id,publisher_id,description,language,maturityRating,pageCount,publishedDate,retailPrice) values (11,'HarryPotter',3000,2000,'Fiction Book into the world of harry potter','English','Mature',420,'12-09-2008',88.31)