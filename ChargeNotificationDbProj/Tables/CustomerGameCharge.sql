CREATE TABLE [dbo].[CustomerGameCharge]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [GameName] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL, 
    [TotalCost] INT NOT NULL, 
    [ChargeDate] DATE NOT NULL, 
    [CustomerId] INT NOT NULL, 
    CONSTRAINT [FK_CustomerGameCharge_ToTable] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
    CONSTRAINT CHK_TotalCost_Positive CHECK ([TotalCost] > 0), 
    CONSTRAINT [UQ_CustomerGameCharge_UniqueEntry] UNIQUE ([CustomerId], [GameName], [ChargeDate]), 
    CONSTRAINT [FK_CustomerGameCharge_ToCustomer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]) ON DELETE CASCADE, 
)

GO

CREATE INDEX [IDX_CustomerGameCharge_ChargeDate] ON [dbo].[CustomerGameCharge] ([ChargeDate])
