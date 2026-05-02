CREATE TABLE [Branch].[tblOwnerSettlement] (
    [SettlementId] INT             IDENTITY (1, 1) NOT NULL,
    [EntityId]     INT             NULL,
    [OwnerId]      INT             NULL,
    [BranchId]     INT             NULL,
    [Amount]       DECIMAL (18, 2) NULL,
    [SettledAt]    DATETIME        NULL,
    [Notes]        NVARCHAR (500)  NULL,
    [SettledBy]    INT             NULL,
    PRIMARY KEY CLUSTERED ([SettlementId] ASC)
);
GO

