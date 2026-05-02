CREATE TABLE [Card].[tblCard] (
    [CardId]               INT             IDENTITY (1, 1) NOT NULL,
    [UserId]               INT             NULL,
    [AvailableAmount]      DECIMAL (18, 2) NULL,
    [LastTransactionId]    NVARCHAR (50)   NULL,
    [CurrentSessionId]     INT             NULL,
    [CurrentCheckInStopId] INT             NULL,
    [CheckInAt]            DATETIME        NULL,
    [IsActive]             BIT             NULL,
    [LastUse]              DATETIME        NULL,
    [DeactivatedAt]        DATETIME        NULL,
    [SectorKey]            VARBINARY (100) NULL,
    PRIMARY KEY CLUSTERED ([CardId] ASC)
);
GO

