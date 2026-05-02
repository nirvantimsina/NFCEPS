CREATE TABLE [Transaction].[tblUserPaymentHistory] (
    [PayId]          INT             IDENTITY (1, 1) NOT NULL,
    [UserId]         INT             NULL,
    [MachineId]      INT             NULL,
    [CheckInStopId]  INT             NULL,
    [CheckOutStopId] INT             NULL,
    [CheckInAt]      DATETIME        NULL,
    [CheckOutAt]     DATETIME        NULL,
    [Fare]           DECIMAL (18, 2) NULL,
    [SessionId]      INT             NULL,
    [Status]         NVARCHAR (20)   NULL,
    PRIMARY KEY CLUSTERED ([PayId] ASC)
);
GO

