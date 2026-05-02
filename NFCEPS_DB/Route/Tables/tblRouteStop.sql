CREATE TABLE [Route].[tblRouteStop] (
    [RouteStopId] INT IDENTITY (1, 1) NOT NULL,
    [RouteId]     INT NULL,
    [StopId]      INT NULL,
    [StopOrder]   INT NULL,
    PRIMARY KEY CLUSTERED ([RouteStopId] ASC)
);
GO

