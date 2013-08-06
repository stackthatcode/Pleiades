
USE CommerceUI
GO

SELECT * FROM Categories



WITH CTE ( ParentId, Id, Name ) AS 
( 
	SELECT   Id, Id, Name 
	FROM     dbo.Categories 
	
	UNION ALL 
   
	SELECT   categories.Id, CTE.Id, categories.Name
	FROM     CTE INNER JOIN dbo.Categories AS categories ON categories.ParentId = cte.ParentId 
)
SELECT  ISNULL(Id, 0) AS ParentId , ISNULL(ParentId, 0) AS Id, Name
FROM    CTE
WHERE Id = 16



WITH CTE ( ParentId, Id, Name ) 
      AS ( SELECT   Id, Id, Name 
           FROM     dbo.Categories 
           
           UNION ALL 
           
           SELECT   CTE.Id, categories.Id, categories.Name
           FROM     CTE 
                    INNER JOIN dbo.Categories AS categories ON categories.ParentId = cte.Id 
         ) 
SELECT  ISNULL(Id, 0) AS Id , ISNULL(ParentId, 0) AS ParentId, Name
FROM    CTE
WHERE Id <> ParentId
AND ParentId = 1









-- https://www.simple-talk.com/sql/t-sql-programming/sql-server-cte-basics/



IF OBJECT_ID('Employees', 'U') IS NOT NULL
DROP TABLE dbo.Employees
GO
CREATE TABLE dbo.Employees
(
  EmployeeID int NOT NULL PRIMARY KEY,
  FirstName varchar(50) NOT NULL,
  LastName varchar(50) NOT NULL,
  ManagerID int NULL
)
GO
INSERT INTO Employees VALUES (101, 'Ken', 'Sánchez', NULL)
INSERT INTO Employees VALUES (102, 'Terri', 'Duffy', 101)
INSERT INTO Employees VALUES (103, 'Roberto', 'Tamburello', 101)
INSERT INTO Employees VALUES (104, 'Rob', 'Walters', 102)
INSERT INTO Employees VALUES (105, 'Gail', 'Erickson', 102)
INSERT INTO Employees VALUES (106, 'Jossef', 'Goldberg', 103)
INSERT INTO Employees VALUES (107, 'Dylan', 'Miller', 103)
INSERT INTO Employees VALUES (108, 'Diane', 'Margheim', 105)
INSERT INTO Employees VALUES (109, 'Gigi', 'Matthew', 105)
INSERT INTO Employees VALUES (110, 'Michael', 'Raheem', 106)


WITH cteReports (EmpID, FirstName, LastName, MgrID, EmpLevel)
  AS
  (
    SELECT EmployeeID, FirstName, LastName, ManagerID, 1
    FROM Employees
    WHERE ManagerID IS NULL
    
    UNION ALL
    
    SELECT e.EmployeeID, e.FirstName, e.LastName, e.ManagerID, r.EmpLevel + 1
    FROM Employees e
      INNER JOIN cteReports r
        ON e.ManagerID = r.EmpID
  )
SELECT
  FirstName + ' ' + LastName AS FullName, EmpLevel,
  (
	SELECT FirstName + ' ' + LastName FROM Employees
    WHERE EmployeeID = cteReports.MgrID
  ) AS Manager
FROM cteReports
ORDER BY EmpLevel, MgrID






