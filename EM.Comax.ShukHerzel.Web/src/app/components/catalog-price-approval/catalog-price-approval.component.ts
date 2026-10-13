import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CatalogPriceApprovalService } from '../../services/catalog-price-approval.service';
import { PendingCatalogPrice } from '../../models/pending-catalog-price';
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
  selector: 'app-catalog-price-approval',
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
  templateUrl: './catalog-price-approval.component.html',
  styleUrl: './catalog-price-approval.component.scss'
})
export class CatalogPriceApprovalComponent extends ApprovalPageBase<PendingCatalogPrice> {
  protected override readonly api = inject(CatalogPriceApprovalService);
  protected override readonly text: ApprovalPageText = {
    successEntity: 'פריטים'
  };

  protected override matchesSearch(item: PendingCatalogPrice, query: string): boolean {
    return (item.barcode ?? '').toLowerCase().includes(query)
      || (item.name ?? '').toLowerCase().includes(query);
  }
}
