-- Run on SQL Server before deploying code changes
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Temp'
      AND t.name = N'PriceUpdates'
      AND c.name = N'OperTransferApprovedByUserId'
)
BEGIN
    ALTER TABLE [Temp].[PriceUpdates]
    ADD [OperTransferApprovedByUserId] INT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Temp'
      AND t.name = N'promotions'
      AND c.name = N'OperTransferApprovedByUserId'
)
BEGIN
    ALTER TABLE [Temp].[promotions]
    ADD [OperTransferApprovedByUserId] INT NULL;
END
GO
