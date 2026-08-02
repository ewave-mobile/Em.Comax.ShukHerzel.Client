import { Routes } from '@angular/router';
import { PriceApprovalComponent } from './features/price-approval/price-approval.component';

export const routes: Routes = [
  { path: '', component: PriceApprovalComponent },
  { path: '**', redirectTo: '' }
];
