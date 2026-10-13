import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './app-config.service';
import { PendingCatalogPrice } from '../models/pending-catalog-price';

@Injectable({ providedIn: 'root' })
export class CatalogPriceApprovalService {
  private readonly http = inject(HttpClient);
  private readonly appConfig = inject(AppConfigService);

  private get baseUrl(): string {
    return this.appConfig.apiBaseUrl;
  }

  getPending(branchId: number, date?: string): Observable<PendingCatalogPrice[]> {
    let params = new HttpParams();
    if (date) {
      params = params.set('date', date);
    }

    return this.http.get<PendingCatalogPrice[]>(
      `${this.baseUrl}/api/branches/${branchId}/catalog-price-updates/pending`,
      { params }
    );
  }

  approve(branchId: number, ids: number[]): Observable<{ approved: number }> {
    return this.http.post<{ approved: number }>(
      `${this.baseUrl}/api/branches/${branchId}/catalog-price-updates/approve`,
      { ids }
    );
  }
}
