export interface Branch {
  id: number;
  branchName: string;
  comaxStoreId: number | null;
  description: string | null;
  isOperTransferApprovalRequired: boolean;
  isCatalogPriceApprovalRequired: boolean;
}
