CREATE TABLE [Branch].[tblCardRecharge] (
    [RechargeId]  INT             IDENTITY (1, 1) NOT NULL,
    [BranchId]    INT             NULL,
    [CardId]      INT             NULL,
    [UserId]      INT             NULL,
    [Amount]      DECIMAL (18, 2) NULL,
    [RechargedAt] DATETIME        NULL,
    [RechargedBy] INT             NULL,
    PRIMARY KEY CLUSTERED ([RechargeId] ASC)
);
GO

