CREATE PROCEDURE [dbo].[PopulateCustomers]
AS
BEGIN
    DECLARE @count INT = 1;

    WHILE @count <= 40000
    BEGIN
        INSERT INTO dbo.Customer (Id, Name)
        VALUES (@count, 'Customer' + CONVERT(VARCHAR(10), @count));

        SET @count = @count + 1;
    END;
END;
