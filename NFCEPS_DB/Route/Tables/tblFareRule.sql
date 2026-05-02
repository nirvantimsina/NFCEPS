CREATE TABLE [Route].[tblFareRule] (
    [FareId]     INT             IDENTITY (1, 1) NOT NULL,
    [RouteId]    INT             NULL,
    [FromStopId] INT             NULL,
    [ToStopId]   INT             NULL,
    [Fare]       DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([FareId] ASC)
);
GO

