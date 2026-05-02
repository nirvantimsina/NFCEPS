CREATE TABLE [Machine].[tblMachine] (
    [MachineId]       INT             IDENTITY (1, 1) NOT NULL,
    [EntityId]        INT             NULL,
    [ApiKey]          VARBINARY (100) NULL,
    [ApiKeyExpiresAt] DATETIME        NULL,
    [MachineLocation] NVARCHAR (200)  NULL,
    [IsActive]        BIT             NULL,
    [LastAuthAt]      DATETIME        NULL,
    [LastSyncAt]      DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([MachineId] ASC)
);
GO

