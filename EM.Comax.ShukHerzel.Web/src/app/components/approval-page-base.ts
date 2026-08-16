import { Directive, OnInit, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BranchStoreService } from '../services/branch-store.service';
import {
  ApprovalPageText,
  PendingApprovalApi,
  PendingApprovalItem
} from '../models/pending-approval';
import { todayKey } from '../pipes/comax-date.pipe';
import { getApiErrorMessage } from '../interceptors/api-error';

@Directive()
export abstract class ApprovalPageBase<T extends PendingApprovalItem> implements OnInit {
  protected readonly store = inject(BranchStoreService);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);

  protected abstract readonly api: PendingApprovalApi<T>;
  protected abstract readonly text: ApprovalPageText;
  protected abstract matchesSearch(item: T, query: string): boolean;

  readonly branchId = signal<number | null>(null);
  readonly items = signal<T[]>([]);
  readonly selectedIds = signal<ReadonlySet<number>>(new Set<number>());
  readonly searchFilter = signal('');
  readonly dateFilter = signal(todayKey());
  readonly loadingItems = signal(false);
  readonly approving = signal(false);
  readonly error = signal<string | null>(null);
  readonly success = signal<string | null>(null);

  readonly branch = computed(() => this.store.byId(this.branchId()));

  readonly filteredItems = computed(() => {
    const query = this.searchFilter().trim().toLowerCase();
    return query
      ? this.items().filter(item => this.matchesSearch(item, query))
      : this.items();
  });

  readonly foundCount = computed(() => this.filteredItems().length);

  readonly selectedCount = computed(() => {
    const ids = this.selectedIds();
    return this.filteredItems().filter(item => ids.has(item.id)).length;
  });

  readonly allSelected = computed(() => {
    const visible = this.filteredItems();
    const ids = this.selectedIds();
    return visible.length > 0 && visible.every(item => ids.has(item.id));
  });

  ngOnInit(): void {
    const branchId = Number(this.route.snapshot.paramMap.get('branchId'));
    if (!Number.isInteger(branchId) || branchId <= 0) {
      void this.router.navigate(['/']);
      return;
    }

    this.branchId.set(branchId);
    this.store.load().subscribe({
      next: branches => {
        if (!branches.some(branch => branch.id === branchId)) {
          void this.router.navigate(['/']);
        }
      },
      error: err => this.error.set(getApiErrorMessage(err))
    });

    this.loadPending();
  }

  back(): void {
    void this.router.navigate(['/branch', this.branchId()]);
  }

  onDateChange(date: string): void {
    this.dateFilter.set(date || todayKey());
    this.success.set(null);
    this.searchFilter.set('');
    this.loadPending();
  }

  refresh(): void {
    if (!this.loadingItems() && this.branchId() != null) {
      this.success.set(null);
      this.loadPending();
    }
  }

  loadPending(): void {
    const branchId = this.branchId();
    if (branchId == null) {
      this.items.set([]);
      return;
    }

    this.loadingItems.set(true);
    this.error.set(null);
    this.selectedIds.set(new Set<number>());

    this.api.getPending(branchId, this.dateFilter()).subscribe({
      next: items => {
        this.items.set(items);
        this.loadingItems.set(false);
      },
      error: err => {
        this.loadingItems.set(false);
        this.error.set(getApiErrorMessage(err));
      }
    });
  }

  isSelected(item: T): boolean {
    return this.selectedIds().has(item.id);
  }

  toggleItem(item: T): void {
    const next = new Set(this.selectedIds());
    next.has(item.id) ? next.delete(item.id) : next.add(item.id);
    this.selectedIds.set(next);
  }

  toggleSelectAll(checked: boolean): void {
    const next = new Set(this.selectedIds());
    for (const item of this.filteredItems()) {
      checked ? next.add(item.id) : next.delete(item.id);
    }
    this.selectedIds.set(next);
  }

  broadcast(): void {
    const branchId = this.branchId();
    if (branchId == null || this.selectedCount() === 0 || this.approving()) {
      return;
    }

    const selectedIds = this.selectedIds();
    const ids = this.filteredItems()
      .filter(item => selectedIds.has(item.id))
      .map(item => item.id);

    this.approving.set(true);
    this.error.set(null);
    this.success.set(null);

    this.api.approve(branchId, ids).subscribe({
      next: result => {
        this.approving.set(false);
        this.success.set(`שודרו ${result.approved} ${this.text.successEntity}`);
        this.loadPending();
      },
      error: err => {
        this.approving.set(false);
        this.error.set(getApiErrorMessage(err));
      }
    });
  }
}
