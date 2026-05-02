CREATE TABLE [Branch].[tblBranch] (
    [BranchId]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NULL,
    [Address]   NVARCHAR (200) NULL,
    [Phone]     NVARCHAR (20)  NULL,
    [CreatedAt] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([BranchId] ASC)
);
GO

