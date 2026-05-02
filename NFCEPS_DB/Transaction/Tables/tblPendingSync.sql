CREATE TABLE [Transaction].[tblPendingSync] (
    [SyncId]       INT            IDENTITY (1, 1) NOT NULL,
    [MachineId]    INT            NULL,
    [SessionId]    INT            NULL,
    [Payload]      NVARCHAR (MAX) NULL,
    [ReceivedAt]   DATETIME       NULL,
    [RetryCount]   INT            NULL,
    [LastTriedAt]  DATETIME       NULL,
    [ErrorMessage] NVARCHAR (500) NULL,
    [Status]       NVARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([SyncId] ASC)
);
GO

