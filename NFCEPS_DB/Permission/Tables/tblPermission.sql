CREATE TABLE [Permission].[tblPermission] (
    [PermId]  INT           IDENTITY (1, 1) NOT NULL,
    [PermKey] NVARCHAR (20) NULL,
    [Label]   NVARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([PermId] ASC)
);
GO

