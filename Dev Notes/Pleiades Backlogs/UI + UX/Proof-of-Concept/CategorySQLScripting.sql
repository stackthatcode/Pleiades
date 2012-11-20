/*
INSERT INTO dbo.Category VALUES ( 1, 'Men''s Section', null );
	
	INSERT INTO dbo.Category VALUES ( 2, 'Shoes', 1 );
	
		INSERT INTO dbo.Category VALUES ( 3, 'Ass-Kicking Shooes', 2 );
		INSERT INTO dbo.Category VALUES ( 4, 'Golf Shoes', 2 );
		INSERT INTO dbo.Category VALUES ( 5, 'Shoes For Going To Park With Sophia', 2 );
	
	INSERT INTO dbo.Category VALUES ( 6, 'Gloves', 1 );

		INSERT INTO dbo.Category VALUES ( 7, 'MMA', 6 );
		INSERT INTO dbo.Category VALUES ( 8, 'Golf', 6 );
		INSERT INTO dbo.Category VALUES ( 9, 'Boxing', 6 );
		
INSERT INTO dbo.Category VALUES ( 10, 'Women''s Section', null );

	INSERT INTO dbo.Category VALUES ( 11, 'Shirts', 10 );
		
		INSERT INTO dbo.Category VALUES ( 12, 'Blouses', 11 );
		INSERT INTO dbo.Category VALUES ( 13, 'Tanktops', 11 );

	INSERT INTO dbo.Category VALUES ( 14, 'Hats', 10 );

		INSERT INTO dbo.Category VALUES ( 12, 'Straw', 14 );
		INSERT INTO dbo.Category VALUES ( 13, 'Felt Top', 14 );

*/


-- by Category
SELECT t2.Id, t2.ParentId, t2.Name FROM Category t1 INNER JOIN Category t2 ON t2.ParentId = t1.Id
WHERE t2.Id = 2 OR t2.ParentId = 2

-- by Section
SELECT 
	t1.Id AS ParentRecordId, 
	t1.ParentId AS ParentRecordParentId, 
	t1.Name AS ParentName,
	t2.Id AS MyId, 
	t2.ParentId AS MyParentId, 
	t2.Name AS MyName
FROM Category t1 
	INNER JOIN Category t2 ON t2.ParentId = t1.Id
WHERE t1.Id = 10 OR t1.ParentId = 10


