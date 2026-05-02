CREATE TABLE [Card].[tblCardHistory] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [CardId]          INT             NULL,
    [UserId]          INT             NULL,
    [TransactionAt]   DATETIME        NULL,
    [PayId]           INT             NULL,
    [RechargeId]      INT             NULL,
    [Amount]          DECIMAL (18, 2) NULL,
    [BalanceAfter]    DECIMAL (18, 2) NULL,
    [TransactionType] NVARCHAR (20)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

