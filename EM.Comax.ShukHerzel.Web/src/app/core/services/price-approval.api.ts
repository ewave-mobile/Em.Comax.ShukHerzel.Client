import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Branch } from '../../shared/models/branch';
import { PendingPriceUpdate } from '../../shared/models/pending-price-update';

@Injectable({ providedIn: 'root' })
export class PriceApprovalApi {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  getBranches(): Observable<Branch[]> {
    return this.http.get<Branch[]>(`${this.baseUrl}/api/branches`);
  }

  getPending(branchId: number): Observable<PendingPriceUpdate[]> {
    return this.http.get<PendingPriceUpdate[]>(
      `${this.baseUrl}/api/branches/${branchId}/price-updates/pending`
    );
  }

  approve(branchId: number, ids: number[]): Observable<{ approved: number }> {
    return this.http.post<{ approved: number }>(
      `${this.baseUrl}/api/branches/${branchId}/price-updates/approve`,
      { ids }
    );
  }
}
