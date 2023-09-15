CREATE TABLE Author (
auth_id  int IDENTITY(3000,1) PRIMARY KEY,
author_name VARCHAR(50)
);

CREATE TABLE Publisher (
pub_id  int IDENTITY(2000,1) PRIMARY KEY,
publisher_name VARCHAR(50)
);


CREATE TABLE Books (
    id INT PRIMARY KEY,
    title VARCHAR(50),
    author_id INT,
    publisher_id INT,
    description VARCHAR(1000),
    FOREIGN KEY (author_id) REFERENCES Author(auth_id),
    FOREIGN KEY (publisher_id) REFERENCES Publisher(pub_id),
     language VARCHAR(20),
    maturityRating VARCHAR(30),
    pageCount INT,
    publishedDate VARCHAR(50),
    retailPrice DECIMAL(6, 3)
);

