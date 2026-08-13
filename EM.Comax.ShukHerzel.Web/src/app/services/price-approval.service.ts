import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './app-config.service';
import { Branch } from '../models/branch';
import { PendingPriceUpdate } from '../models/pending-price-update';

@Injectable({ providedIn: 'root' })
export class PriceApprovalService {
  private readonly http = inject(HttpClient);
  private readonly appConfig = inject(AppConfigService);

  private get baseUrl(): string {
    return this.appConfig.apiBaseUrl;
  }

  getBranches(): Observable<Branch[]> {
    return this.http.get<Branch[]>(`${this.baseUrl}/api/branches`);
  }

  getPending(branchId: number, date?: string): Observable<PendingPriceUpdate[]> {
    let params = new HttpParams();
    if (date) {
      params = params.set('date', date);
    }

    return this.http.get<PendingPriceUpdate[]>(
      `${this.baseUrl}/api/branches/${branchId}/price-updates/pending`,
      { params }
    );
  }

  approve(branchId: number, ids: number[]): Observable<{ approved: number }> {
    return this.http.post<{ approved: number }>(
      `${this.baseUrl}/api/branches/${branchId}/price-updates/approve`,
      { ids }
    );
  }
}
