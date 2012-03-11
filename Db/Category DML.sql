
USE PleiadesDB
GO

DELETE FROM Category
INSERT INTO Category VALUES ( 'Helmets', 'Lids for your dome', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Snowboards', 'Get wrapped around a nice tree', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Hanggliders', 'Fly like the wind', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Belts', 'Get the biggest belt buckle in Texas', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Shoes', 'Look and act the part, be professional', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Rope', 'Rock climbing gear', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Gloves', 'Top quality for real enthusiast', 1, GETDATE(), GETDATE() )


SELECT * FROM Category;


SELECT * FROM DomainUsers

/*
DELETE FROM DomainUsers
DELETE FROM Users
*/

SELECT LastActivityDate FROM Users WHERE Username = 'admin'

/*
DELETE FROM Category
INSERT INTO Category VALUES ( 'Helmets', 'Lids for your dome', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Snowboards', 'Get wrapped around a nice tree', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Hanggliders', 'Fly like the wind', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Belts', 'Get the biggest belt buckle in Texas', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Shoes', 'Look and act the part, be professional', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Rope', 'Rock climbing gear', 1, GETDATE(), GETDATE() )
INSERT INTO Category VALUES ( 'Gloves', 'Top quality for real enthusiast', 1, GETDATE(), GETDATE() )
*/



UPDATE DomainUsers SET UserRole = 'Root' WHERE DomainUserId = 244


