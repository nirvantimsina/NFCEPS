CREATE TABLE [Route].[tblStop] (
    [StopId]   INT            IDENTITY (1, 1) NOT NULL,
    [StopName] NVARCHAR (100) NULL,
    [IsActive] BIT            NULL,
    PRIMARY KEY CLUSTERED ([StopId] ASC)
);
GO

