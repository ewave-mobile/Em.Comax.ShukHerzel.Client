import { Observable } from 'rxjs';

export interface PendingApprovalItem {
  id: number;
}

export interface PendingApprovalApi<T extends PendingApprovalItem> {
  getPending(branchId: number, date?: string): Observable<T[]>;
  approve(branchId: number, ids: number[]): Observable<{ approved: number }>;
}

export interface ApprovalPageText {
  loadError: string;
  approveError: string;
  successEntity: string;
}
