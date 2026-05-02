CREATE TABLE [Entity].[tblEntityOwner] (
    [OwnerId]   INT            IDENTITY (1, 1) NOT NULL,
    [Address_1] NVARCHAR (200) NULL,
    [Address_2] NVARCHAR (200) NULL,
    [Name]      NVARCHAR (100) NULL,
    [Phone]     NVARCHAR (100) NULL,
    [CreatedAt] DATETIME       NULL,
    [IsActive]  BIT            NULL,
    PRIMARY KEY CLUSTERED ([OwnerId] ASC)
);
GO

