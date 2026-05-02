CREATE TABLE [Permission].[tblRolePermission] (
    [RoleId]    INT NOT NULL,
    [PermId]    INT NOT NULL,
    [IsAllowed] BIT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermId] ASC)
);
GO

