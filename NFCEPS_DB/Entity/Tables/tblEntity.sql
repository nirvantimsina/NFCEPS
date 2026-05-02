CREATE TABLE [Entity].[tblEntity] (
    [EntityId]       INT            IDENTITY (1, 1) NOT NULL,
    [EntityName]     NVARCHAR (200) NULL,
    [OwnerId]        INT            NULL,
    [CreatedAt]      DATETIME       NULL,
    [EntityLocation] NVARCHAR (200) NULL,
    [IsActive]       BIT            NULL,
    PRIMARY KEY CLUSTERED ([EntityId] ASC)
);
GO

