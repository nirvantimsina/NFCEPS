CREATE TABLE [Permission].[tblRoles] (
    [RoleId]   INT           IDENTITY (1, 1) NOT NULL,
    [RoleName] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
);
GO

