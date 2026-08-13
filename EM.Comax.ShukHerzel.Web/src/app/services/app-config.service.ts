import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';

export interface RuntimeConfig {
  apiBaseUrl: string;
}

@Injectable({ providedIn: 'root' })
export class AppConfigService {
  private readonly http = inject(HttpClient);
  private config: RuntimeConfig = {
    apiBaseUrl: environment.apiBaseUrl ?? ''
  };

  get apiBaseUrl(): string {
    return (this.config.apiBaseUrl ?? '').replace(/\/$/, '');
  }

  load(): Promise<void> {
    return firstValueFrom(this.http.get<RuntimeConfig>('config.json', { headers: { 'Cache-Control': 'no-cache' } }))
      .then(cfg => {
        if (cfg && typeof cfg.apiBaseUrl === 'string') {
          this.config = { apiBaseUrl: cfg.apiBaseUrl };
        }
      })
      .catch(() => {
        // Keep environment fallback if config.json is missing
      });
  }
}
