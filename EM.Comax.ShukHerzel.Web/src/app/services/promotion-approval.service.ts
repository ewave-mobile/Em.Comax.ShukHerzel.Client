import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './app-config.service';
import { PendingPromotion } from '../models/pending-promotion';

@Injectable({ providedIn: 'root' })
export class PromotionApprovalService {
  private readonly http = inject(HttpClient);
  private readonly appConfig = inject(AppConfigService);

  private get baseUrl(): string {
    return this.appConfig.apiBaseUrl;
  }

  getPending(branchId: number, date?: string): Observable<PendingPromotion[]> {
    let params = new HttpParams();
    if (date) {
      params = params.set('date', date);
    }

    return this.http.get<PendingPromotion[]>(
      `${this.baseUrl}/api/branches/${branchId}/promotions/pending`,
      { params }
    );
  }

  approve(branchId: number, ids: number[]): Observable<{ approved: number }> {
    return this.http.post<{ approved: number }>(
      `${this.baseUrl}/api/branches/${branchId}/promotions/approve`,
      { ids }
    );
  }

  reject(branchId: number, ids: number[]): Observable<{ rejected: number }> {
    return this.http.post<{ rejected: number }>(
      `${this.baseUrl}/api/branches/${branchId}/promotions/reject`,
      { ids }
    );
  }
}
