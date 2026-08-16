import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { toApiError } from './api-error';

export const apiErrorInterceptor: HttpInterceptorFn = (req, next) => {
  if (req.url.includes('/assets/') || req.url.startsWith('assets/')) {
    return next(req);
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => throwError(() => toApiError(error)))
  );
};
