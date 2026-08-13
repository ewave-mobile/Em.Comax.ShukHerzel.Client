import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceApprovalService } from '../../services/price-approval.service';
import { PendingPriceUpdate } from '../../models/pending-price-update';
import { AppPageShellComponent } from '../../shared/components/app-page-shell/app-page-shell.component';
import { AppPageHeaderComponent } from '../../shared/components/app-page-header/app-page-header.component';
import { ApprovalFiltersComponent } from '../../shared/components/approval-filters/approval-filters.component';
import { SelectionToolbarComponent } from '../../shared/components/selection-toolbar/selection-toolbar.component';
import { PageStatusComponent } from '../../shared/components/page-status/page-status.component';
import { FooterActionComponent } from '../../shared/components/footer-action/footer-action.component';
import { ApprovalPageBase } from '../../shared/components/approval-page-base';
import { ApprovalPageText } from '../../shared/models/pending-approval';
import { formatPrice } from '../../shared/utils/formatters';

@Component({
  selector: 'app-price-approval',
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
  templateUrl: './price-approval.component.html',
  styleUrl: './price-approval.component.css'
})
export class PriceApprovalComponent extends ApprovalPageBase<PendingPriceUpdate> {
  protected override readonly api = inject(PriceApprovalService);
  protected override readonly text: ApprovalPageText = {
    loadError: 'שגיאה בטעינת עדכוני מחירים',
    approveError: 'שגיאה בשידור הפריטים',
    successEntity: 'פריטים'
  };

  readonly formatPrice = formatPrice;

  protected override matchesSearch(item: PendingPriceUpdate, query: string): boolean {
    return (item.barcode ?? '').toLowerCase().includes(query)
      || (item.name ?? '').toLowerCase().includes(query);
  }
}
