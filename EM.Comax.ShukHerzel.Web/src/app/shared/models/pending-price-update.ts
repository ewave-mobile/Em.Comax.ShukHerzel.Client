export interface PendingPriceUpdate {
  id: number;
  name: string | null;
  barcode: string | null;
  netPrice: string | null;
  createdDateTime: string;
  selected?: boolean;
}
