CREATE TABLE [dbo].[CustomerGameCharge]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [GameName] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL, 
    [TotalCost] INT NOT NULL, 
    [ChargeDate] DATE NOT NULL, 
    [CustomerId] INT NOT NULL, 
    CONSTRAINT [FK_CustomerGameCharge_ToTable] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
)

GO

CREATE INDEX [IDX_CustomerGameCharge_ChargeDate] ON [dbo].[CustomerGameCharge] ([ChargeDate])
