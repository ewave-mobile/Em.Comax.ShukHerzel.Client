export interface PendingCatalogPrice {
  id: number;
  barcode: string | null;
  name: string | null;
  oldPrice: number | null;
  newPrice: string | null;
  createdDateTime: string;
}
