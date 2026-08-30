import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { AppConfigService } from './app-config.service';

const TOKEN_KEY = 'shukherzel_access_token';

export interface LoginResponse {
  requiresOtp: boolean;
}

export interface VerifyResponse {
  accessToken: string;
  expiresIn: number;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly appConfig = inject(AppConfigService);
  private readonly router = inject(Router);

  private readonly tokenSignal = signal<string | null>(this.readStoredToken());

  readonly isAuthenticated = computed(() => !!this.tokenSignal());
  readonly userName = computed(() => this.readUserNameFromToken(this.tokenSignal()));

  get token(): string | null {
    return this.tokenSignal();
  }

  private get baseUrl(): string {
    return this.appConfig.apiBaseUrl;
  }

  login(userName: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/api/auth/login`, {
      userName,
      password
    });
  }

  verify(userName: string, code: string): Observable<VerifyResponse> {
    return this.http.post<VerifyResponse>(`${this.baseUrl}/api/auth/verify`, {
      userName,
      code
    }).pipe(
      tap(res => this.setToken(res.accessToken))
    );
  }

  logout(navigateToLogin = true): void {
    localStorage.removeItem(TOKEN_KEY);
    this.tokenSignal.set(null);
    if (navigateToLogin) {
      void this.router.navigateByUrl('/login');
    }
  }

  private setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
    this.tokenSignal.set(token);
  }

  private readStoredToken(): string | null {
    try {
      return localStorage.getItem(TOKEN_KEY);
    } catch {
      return null;
    }
  }

  private readUserNameFromToken(token: string | null): string | null {
    if (!token) {
      return null;
    }

    try {
      const payload = this.decodeJwtPayload(token);
      if (!payload) {
        return null;
      }

      const firstName = typeof payload['first_name'] === 'string' ? payload['first_name'].trim() : '';
      const lastName = typeof payload['last_name'] === 'string' ? payload['last_name'].trim() : '';
      const fullName = [firstName, lastName].filter(Boolean).join(' ');
      if (fullName) {
        return fullName;
      }

      const candidates = [
        payload['name'],
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        payload['unique_name']
      ];

      for (const value of candidates) {
        if (typeof value === 'string' && value.trim()) {
          return value.trim();
        }
      }

      return null;
    } catch {
      return null;
    }
  }

  /** JWT payload is UTF-8 JSON; plain atob breaks Hebrew names. */
  private decodeJwtPayload(token: string): Record<string, unknown> | null {
    const payloadPart = token.split('.')[1];
    if (!payloadPart) {
      return null;
    }

    const base64 = payloadPart.replace(/-/g, '+').replace(/_/g, '/');
    const padded = base64 + '='.repeat((4 - (base64.length % 4)) % 4);
    const binary = atob(padded);
    const bytes = Uint8Array.from(binary, char => char.charCodeAt(0));
    const json = new TextDecoder('utf-8').decode(bytes);
    return JSON.parse(json) as Record<string, unknown>;
  }
}
