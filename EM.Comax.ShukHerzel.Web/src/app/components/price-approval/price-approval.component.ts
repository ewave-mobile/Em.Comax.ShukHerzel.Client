import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceApprovalService } from '../../services/price-approval.service';
import { PendingPriceUpdate } from '../../models/pending-price-update';
import { AppPageShellComponent } from '../app-page-shell/app-page-shell.component';
import { AppPageHeaderComponent } from '../app-page-header/app-page-header.component';
import { ApprovalFiltersComponent } from '../approval-filters/approval-filters.component';
import { SelectionToolbarComponent } from '../selection-toolbar/selection-toolbar.component';
import { PageStatusComponent } from '../page-status/page-status.component';
import { FooterActionComponent } from '../footer-action/footer-action.component';
import { ApprovalPageBase } from '../approval-page-base';
import { ApprovalPageText } from '../../models/pending-approval';
import { PricePipe } from '../../pipes/price.pipe';

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
    FooterActionComponent,
    PricePipe
  ],
  templateUrl: './price-approval.component.html',
  styleUrl: './price-approval.component.scss'
})
export class PriceApprovalComponent extends ApprovalPageBase<PendingPriceUpdate> {
  protected override readonly api = inject(PriceApprovalService);
  protected override readonly text: ApprovalPageText = {
    successEntity: 'פריטים'
  };

  protected override matchesSearch(item: PendingPriceUpdate, query: string): boolean {
    return (item.barcode ?? '').toLowerCase().includes(query)
      || (item.name ?? '').toLowerCase().includes(query);
  }
}
