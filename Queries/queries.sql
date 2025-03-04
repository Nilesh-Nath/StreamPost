CREATE DATABASE StreamPost;

USE StreamPost;

SELECT * FROM categories;
SELECT * FROM comments;
SELECT * FROM posts;
SELECT * FROM AspNetUsers;
SELECT * FROM likes;


SELECT * FROM __EFMigrationsHistory;


SELECT name, delete_referential_action_desc 
FROM sys.foreign_keys 
WHERE name = 'FK_posts_categories_CategoryID';

EXEC sp_fkeys 'posts';

ALTER TABLE posts 
ADD CONSTRAINT FK_posts_categories_CategoryID
FOREIGN KEY (CategoryID) REFERENCES categories(CategoryID) 
ON DELETE NO ACTION;

ALTER TABLE posts DROP CONSTRAINT FK_posts_categories_CategoryID;

ALTER TABLE posts ALTER COLUMN CommentNumber INT NULL;

-----------------------------------------------------------------------

INSERT INTO categories(CategoryName) VALUES ('Technology'),('Sports'),('Politics'),('Social'),('Entertainment'),('Health');

-----------------------------------------------------------------------
--DELETE FROM categories;
--DELETE FROM users;
--DELETE FROM comments;
--DELETE FROM posts;

--DROP TABLE categories;
--DROP TABLE comments;
--DROP TABLE users;
--DROP TABLE posts;

--DELETE FROM  __EFMigrationsHistory