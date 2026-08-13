import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PromotionApprovalService } from '../../services/promotion-approval.service';
import { PendingPromotion } from '../../models/pending-promotion';
import { AppPageShellComponent } from '../../shared/components/app-page-shell/app-page-shell.component';
import { AppPageHeaderComponent } from '../../shared/components/app-page-header/app-page-header.component';
import { ApprovalFiltersComponent } from '../../shared/components/approval-filters/approval-filters.component';
import { SelectionToolbarComponent } from '../../shared/components/selection-toolbar/selection-toolbar.component';
import { PageStatusComponent } from '../../shared/components/page-status/page-status.component';
import { FooterActionComponent } from '../../shared/components/footer-action/footer-action.component';
import { ApprovalPageBase } from '../../shared/components/approval-page-base';
import { ApprovalPageText } from '../../shared/models/pending-approval';
import { formatComaxDate, formatPrice } from '../../shared/utils/formatters';

@Component({
  selector: 'app-promotion-approval',
  imports: [
    MatCardModule,
    MatCheckboxModule,
    AppPageShellComponent,
    AppPageHeaderComponent,
    ApprovalFiltersComponent,
    SelectionToolbarComponent,
    PageStatusComponent,
    FooterActionComponent
  ],
  templateUrl: './promotion-approval.component.html',
  styleUrl: './promotion-approval.component.css'
})
export class PromotionApprovalComponent extends ApprovalPageBase<PendingPromotion> {
  protected override readonly api = inject(PromotionApprovalService);
  protected override readonly text: ApprovalPageText = {
    loadError: 'שגיאה בטעינת תגמולים',
    approveError: 'שגיאה בשידור התגמולים',
    successEntity: 'תגמולים'
  };

  readonly formatPrice = formatPrice;
  readonly formatPromoDate = formatComaxDate;

  protected override matchesSearch(item: PendingPromotion, query: string): boolean {
    return (item.itemKod ?? '').toLowerCase().includes(query)
      || (item.nm ?? '').toLowerCase().includes(query)
      || (item.kod ?? '').toLowerCase().includes(query);
  }
}
