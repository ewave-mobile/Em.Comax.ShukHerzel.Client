import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PriceApprovalApi } from '../../core/services/price-approval.api';
import { Branch } from '../../shared/models/branch';
import { PendingPriceUpdate } from '../../shared/models/pending-price-update';

@Component({
  selector: 'app-price-approval',
  imports: [CommonModule, FormsModule],
  templateUrl: './price-approval.component.html',
  styleUrl: './price-approval.component.css'
})
export class PriceApprovalComponent implements OnInit {
  private readonly api = inject(PriceApprovalApi);

  branches: Branch[] = [];
  selectedBranchId: number | null = null;
  items: PendingPriceUpdate[] = [];
  barcodeFilter = '';
  loadingBranches = false;
  loadingItems = false;
  approving = false;
  error: string | null = null;
  success: string | null = null;

  get selectedBranch(): Branch | undefined {
    return this.branches.find(b => b.id === this.selectedBranchId);
  }

  get filteredItems(): PendingPriceUpdate[] {
    const barcodes = this.parseBarcodes(this.barcodeFilter);
    if (barcodes.length === 0) {
      return this.items;
    }

    const barcodeSet = new Set(barcodes);
    return this.items.filter(i => barcodeSet.has((i.barcode ?? '').trim()));
  }

  get foundCount(): number {
    return this.filteredItems.length;
  }

  get selectedCount(): number {
    return this.filteredItems.filter(i => i.selected).length;
  }

  get allSelected(): boolean {
    return this.filteredItems.length > 0 && this.filteredItems.every(i => i.selected);
  }

  ngOnInit(): void {
    this.loadBranches();
  }

  loadBranches(): void {
    this.loadingBranches = true;
    this.error = null;
    this.api.getBranches().subscribe({
      next: branches => {
        this.branches = branches;
        this.loadingBranches = false;
        if (branches.length > 0) {
          this.selectedBranchId = branches[0].id;
          this.loadPending();
        }
      },
      error: () => {
        this.loadingBranches = false;
        this.error = 'שגיאה בטעינת סניפים';
      }
    });
  }

  onBranchChange(): void {
    this.success = null;
    this.barcodeFilter = '';
    this.loadPending();
  }

  loadPending(): void {
    if (this.selectedBranchId == null) {
      this.items = [];
      return;
    }

    this.loadingItems = true;
    this.error = null;
    this.api.getPending(this.selectedBranchId).subscribe({
      next: items => {
        this.items = items.map(i => ({ ...i, selected: false }));
        this.loadingItems = false;
      },
      error: () => {
        this.loadingItems = false;
        this.error = 'שגיאה בטעינת עדכוני מחירים';
      }
    });
  }

  toggleSelectAll(checked: boolean): void {
    const visibleIds = new Set(this.filteredItems.map(i => i.id));
    this.items = this.items.map(i =>
      visibleIds.has(i.id) ? { ...i, selected: checked } : i
    );
  }

  private parseBarcodes(raw: string): string[] {
    return raw
      .split(',')
      .map(s => s.trim())
      .filter(s => s.length > 0);
  }

  toggleItem(item: PendingPriceUpdate): void {
    item.selected = !item.selected;
  }

  formatPrice(value: string | null): string {
    if (!value) {
      return '—';
    }
    const num = Number(value);
    if (Number.isNaN(num)) {
      return `₪${value}`;
    }
    return `₪${num.toFixed(2)}`;
  }

  formatTime(value: string): string {
    const d = new Date(value);
    if (Number.isNaN(d.getTime())) {
      return '';
    }
    return d.toLocaleTimeString('he-IL', { hour: '2-digit', minute: '2-digit' });
  }

  broadcast(): void {
    if (this.selectedBranchId == null || this.selectedCount === 0 || this.approving) {
      return;
    }

    const ids = this.filteredItems.filter(i => i.selected).map(i => i.id);
    this.approving = true;
    this.error = null;
    this.success = null;

    this.api.approve(this.selectedBranchId, ids).subscribe({
      next: result => {
        this.approving = false;
        this.success = `שודרו ${result.approved} פריטים`;
        this.loadPending();
      },
      error: () => {
        this.approving = false;
        this.error = 'שגיאה בשידור הפריטים';
      }
    });
  }
}
