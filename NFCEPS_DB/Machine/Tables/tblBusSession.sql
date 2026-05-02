CREATE TABLE [Machine].[tblBusSession] (
    [SessionId]     INT           IDENTITY (1, 1) NOT NULL,
    [MachineId]     INT           NULL,
    [RouteId]       INT           NULL,
    [DriverId]      INT           NULL,
    [StartedAt]     DATETIME      NULL,
    [EndedAt]       DATETIME      NULL,
    [CurrentStopId] INT           NULL,
    [Status]        NVARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([SessionId] ASC)
);
GO

