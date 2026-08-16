import { HttpErrorResponse } from '@angular/common/http';

export class ApiError extends Error {
  readonly status: number;
  readonly original?: HttpErrorResponse;

  constructor(message: string, status: number, original?: HttpErrorResponse) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.original = original;
  }
}

export function toApiError(error: unknown): ApiError {
  if (error instanceof ApiError) {
    return error;
  }

  if (error instanceof HttpErrorResponse) {
    return new ApiError(mapHttpError(error), error.status, error);
  }

  return new ApiError('אירעה שגיאה בפנייה לשרת', 0);
}

export function getApiErrorMessage(error: unknown): string {
  return toApiError(error).message;
}

function mapHttpError(error: HttpErrorResponse): string {
  const serverMessage = readServerMessage(error);

  if (error.status === 0) {
    return 'אין תקשורת עם השרת';
  }

  if (error.status === 400) {
    return serverMessage ?? 'הבקשה לא תקינה';
  }

  if (error.status === 401 || error.status === 403) {
    return 'אין הרשאה לבצע פעולה זו';
  }

  if (error.status === 404) {
    return 'המשאב המבוקש לא נמצא';
  }

  if (error.status >= 500) {
    return serverMessage ?? 'שגיאה בשרת. נסו שוב מאוחר יותר';
  }

  return serverMessage ?? 'אירעה שגיאה בפנייה לשרת';
}

function readServerMessage(error: HttpErrorResponse): string | null {
  const body = error.error;

  if (typeof body === 'string' && body.trim()) {
    try {
      const parsed = JSON.parse(body) as { message?: string; title?: string };
      return readMessageFromBody(parsed) ?? body.trim();
    } catch {
      return body.trim();
    }
  }

  if (body && typeof body === 'object') {
    return readMessageFromBody(body as { message?: string; title?: string });
  }

  return null;
}

function readMessageFromBody(body: { message?: string; title?: string }): string | null {
  if (typeof body.message === 'string' && body.message.trim()) {
    return body.message.trim();
  }

  if (typeof body.title === 'string' && body.title.trim()) {
    return body.title.trim();
  }

  return null;
}
