IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Management'
      AND t.name = N'Company'
      AND c.name = N'IsCatalogPriceApprovalRequired'
)
BEGIN
    ALTER TABLE [Management].[Company]
    ADD [IsCatalogPriceApprovalRequired] BIT NOT NULL
        CONSTRAINT [DF_Company_IsCatalogPriceApprovalRequired] DEFAULT (0);
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Temp'
      AND t.name = N'AllItemsComax'
      AND c.name = N'IsCatalogPriceApprovalPending'
)
BEGIN
    ALTER TABLE [Temp].[AllItemsComax]
    ADD [IsCatalogPriceApprovalPending] BIT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Temp'
      AND t.name = N'AllItemsComax'
      AND c.name = N'IsOperTransferApproved'
)
BEGIN
    ALTER TABLE [Temp].[AllItemsComax]
    ADD [IsOperTransferApproved] BIT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Temp'
      AND t.name = N'AllItemsComax'
      AND c.name = N'OperTransferApprovedDate'
)
BEGIN
    ALTER TABLE [Temp].[AllItemsComax]
    ADD [OperTransferApprovedDate] DATETIME NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE s.name = N'Temp'
      AND t.name = N'AllItemsComax'
      AND c.name = N'OperTransferApprovedByUserId'
)
BEGIN
    ALTER TABLE [Temp].[AllItemsComax]
    ADD [OperTransferApprovedByUserId] INT NULL;
END
GO
