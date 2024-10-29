CREATE PROCEDURE [dbo].[PopulateCustomerGameCharge]
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NumberOfRecords INT = 40000
    DECLARE @Counter INT = 5;
    DECLARE @MinCustomerId INT, @MaxCustomerId INT;

    -- Ensure there's an existing range of CustomerIds in the Customer table
    SELECT @MinCustomerId = MIN(Id), @MaxCustomerId = MAX(Id) FROM Customer;

    DECLARE @GameName NVARCHAR(50), 
            @Description NVARCHAR(50), 
            @TotalCost FLOAT, 
            @CustomerId INT;

    DECLARE @ChargeDate DATE = CAST(GETDATE() AS DATE);

    IF @MinCustomerId IS NULL OR @MaxCustomerId IS NULL
    BEGIN
        RAISERROR('No customer records found. Please insert customer records first.', 16, 1);
        RETURN;
    END

    WHILE @Counter <= @NumberOfRecords
    BEGIN
        SET @GameName = 'Game_' + CAST(ABS(CHECKSUM(NEWID())) % 100 + 1 AS NVARCHAR(50));
        SET @Description = 'Description_' + CAST(ABS(CHECKSUM(NEWID())) % 1000 + 1 AS NVARCHAR(50));
        SET @TotalCost = ROUND((RAND() * 100), 2); 
        SET @CustomerId = @MinCustomerId + ABS(CHECKSUM(NEWID())) % (@MaxCustomerId - @MinCustomerId + 1);

        INSERT INTO dbo.CustomerGameCharge (Id, GameName, Description, TotalCost, ChargeDate, CustomerId)
        VALUES (@Counter, @GameName, @Description, @TotalCost, @ChargeDate, @CustomerId);

        SET @Counter = @Counter + 1;
    END;

    PRINT 'Completed populating ' + CAST(@NumberOfRecords AS NVARCHAR(10)) + ' records into CustomerGameCharge.';
END;
