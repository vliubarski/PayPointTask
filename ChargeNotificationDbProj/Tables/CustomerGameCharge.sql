CREATE TABLE [dbo].[CustomerGameCharges]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [GameName] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL, 
    [TotalCost] FLOAT NOT NULL, 
    [ChargeDate] DATE NOT NULL, 
    [CustomerId] INT NOT NULL, 
    CONSTRAINT [FK_CustomerGameCharges_ToTable] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([Id]), 
)
