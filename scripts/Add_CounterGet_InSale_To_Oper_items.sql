-- Run on SQL Server before deploying code changes
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Oper'
      AND t.name = N'items'
      AND c.name = N'CounterGet_InSale'
)
BEGIN
    ALTER TABLE [Oper].[items]
    ADD [CounterGet_InSale] NVARCHAR(50) NULL;
END
