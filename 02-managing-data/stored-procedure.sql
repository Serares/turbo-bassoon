CREATE PROCEDURE [dbo].[GetExpensiveProducts]
@price money,
@count int OUT
AS
PRINT 'Getting expensive products: ' + -- this raises the InfoMessage event
TRIM(CAST(@price AS NVARCHAR(10)))
SELECT @count = COUNT(*)
FROM Products
WHERE UnitPrice >= @price
SELECT *
FROM Products
WHERE UnitPrice >= @price
RETURN 0