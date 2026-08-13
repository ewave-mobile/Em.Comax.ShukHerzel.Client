import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { BranchStoreService } from '../../services/branch-store.service';
import { PriceApprovalService } from '../../services/price-approval.service';
import { PromotionApprovalService } from '../../services/promotion-approval.service';
import { AppPageShellComponent } from '../../shared/components/app-page-shell/app-page-shell.component';
import { AppPageHeaderComponent } from '../../shared/components/app-page-header/app-page-header.component';
import { todayKey } from '../../shared/utils/formatters';

@Component({
  selector: 'app-branch-actions',
  imports: [
    MatButtonModule,
    MatRippleModule,
    MatIconModule,
    AppPageShellComponent,
    AppPageHeaderComponent
  ],
  templateUrl: './branch-actions.component.html',
  styleUrl: './branch-actions.component.css'
})
export class BranchActionsComponent implements OnInit {
  private readonly store = inject(BranchStoreService);
  private readonly priceApi = inject(PriceApprovalService);
  private readonly promotionApi = inject(PromotionApprovalService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly branchId = signal<number | null>(null);
  readonly pendingItemsCount = signal(0);
  readonly pendingPromotionsCount = signal(0);
  readonly branch = computed(() => this.store.byId(this.branchId()));

  ngOnInit(): void {
    const branchId = Number(this.route.snapshot.paramMap.get('branchId'));
    if (!Number.isInteger(branchId) || branchId <= 0) {
      this.changeBranch();
      return;
    }

    this.branchId.set(branchId);
    this.store.load().subscribe({
      next: branches => {
        if (!branches.some(b => b.id === branchId)) {
          this.changeBranch();
          return;
        }
        this.loadPendingCounts(branchId);
      },
      error: () => this.changeBranch()
    });
  }

  private loadPendingCounts(branchId: number): void {
    const date = todayKey();
    this.priceApi.getPending(branchId, date).subscribe({
      next: items => this.pendingItemsCount.set(items.length),
      error: () => this.pendingItemsCount.set(0)
    });
    this.promotionApi.getPending(branchId, date).subscribe({
      next: items => this.pendingPromotionsCount.set(items.length),
      error: () => this.pendingPromotionsCount.set(0)
    });
  }

  openPriceUpdates(): void {
    this.router.navigate(['/branch', this.branchId(), 'price-updates']);
  }

  openPromotions(): void {
    this.router.navigate(['/branch', this.branchId(), 'promotions']);
  }

  changeBranch(): void {
    this.router.navigate(['/']);
  }
}
