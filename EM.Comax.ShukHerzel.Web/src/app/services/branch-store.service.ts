import { Injectable, inject, signal } from '@angular/core';
import { Observable, catchError, shareReplay, tap, throwError } from 'rxjs';
import { PriceApprovalService } from './price-approval.service';
import { Branch } from '../models/branch';

@Injectable({ providedIn: 'root' })
export class BranchStoreService {
  private readonly api = inject(PriceApprovalService);
  private request?: Observable<Branch[]>;

  readonly branches = signal<Branch[]>([]);

  load(): Observable<Branch[]> {
    this.request ??= this.api.getBranches().pipe(
      tap(branches => this.branches.set(branches)),
      catchError(err => {
        this.request = undefined;
        return throwError(() => err);
      }),
      shareReplay({ bufferSize: 1, refCount: false })
    );

    return this.request;
  }

  byId(id: number | null): Branch | undefined {
    if (id == null) {
      return undefined;
    }
    return this.branches().find(b => b.id === id);
  }
}
