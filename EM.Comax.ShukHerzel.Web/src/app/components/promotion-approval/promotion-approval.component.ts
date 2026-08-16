import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PromotionApprovalService } from '../../services/promotion-approval.service';
import { PendingPromotion } from '../../models/pending-promotion';
import { AppPageShellComponent } from '../app-page-shell/app-page-shell.component';
import { AppPageHeaderComponent } from '../app-page-header/app-page-header.component';
import { ApprovalFiltersComponent } from '../approval-filters/approval-filters.component';
import { SelectionToolbarComponent } from '../selection-toolbar/selection-toolbar.component';
import { PageStatusComponent } from '../page-status/page-status.component';
import { FooterActionComponent } from '../footer-action/footer-action.component';
import { ApprovalPageBase } from '../approval-page-base';
import { ApprovalPageText } from '../../models/pending-approval';
import { PricePipe } from '../../pipes/price.pipe';
import { ComaxDatePipe } from '../../pipes/comax-date.pipe';

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
    FooterActionComponent,
    PricePipe,
    ComaxDatePipe
  ],
  templateUrl: './promotion-approval.component.html',
  styleUrl: './promotion-approval.component.scss'
})
export class PromotionApprovalComponent extends ApprovalPageBase<PendingPromotion> {
  protected override readonly api = inject(PromotionApprovalService);
  protected override readonly text: ApprovalPageText = {
    successEntity: 'תגמולים'
  };

  protected override matchesSearch(item: PendingPromotion, query: string): boolean {
    return (item.itemKod ?? '').toLowerCase().includes(query)
      || (item.nm ?? '').toLowerCase().includes(query)
      || (item.kod ?? '').toLowerCase().includes(query);
  }
}
