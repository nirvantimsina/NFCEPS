CREATE TABLE [Route].[tblRoute] (
    [RouteId]   INT            IDENTITY (1, 1) NOT NULL,
    [RouteName] NVARCHAR (100) NULL,
    [EntityId]  INT            NULL,
    [IsActive]  BIT            NULL,
    PRIMARY KEY CLUSTERED ([RouteId] ASC)
);
GO

