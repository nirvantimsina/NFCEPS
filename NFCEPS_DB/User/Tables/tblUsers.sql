CREATE TABLE [User].[tblUsers] (
    [UserId]    INT             IDENTITY (1, 1) NOT NULL,
    [RFID]      NVARCHAR (20)   NULL,
    [UserName]  NVARCHAR (100)  NULL,
    [RoleId]    INT             NULL,
    [Name]      NVARCHAR (100)  NULL,
    [Address]   NVARCHAR (100)  NULL,
    [Phone]     NVARCHAR (20)   NULL,
    [IsActive]  BIT             NULL,
    [Password]  VARBINARY (100) NULL,
    [CreatedAt] DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);
GO

