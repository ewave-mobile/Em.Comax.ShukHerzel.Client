import { ApplicationConfig, inject, provideAppInitializer, provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { routes } from './app.routes';
import { AppConfigService } from './services/app-config.service';
import { apiErrorInterceptor } from './interceptors/api-error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideHttpClient(withInterceptors([apiErrorInterceptor])),
    provideRouter(routes),
    { provide: MAT_DATE_LOCALE, useValue: 'he-IL' },
    provideAppInitializer(() => inject(AppConfigService).load())
  ]
};
