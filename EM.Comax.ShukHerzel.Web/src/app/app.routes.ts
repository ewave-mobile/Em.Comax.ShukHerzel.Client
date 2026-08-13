import { Routes } from '@angular/router';
import { BranchSelectComponent } from './components/branch-select/branch-select.component';
import { BranchActionsComponent } from './components/branch-actions/branch-actions.component';
import { PriceApprovalComponent } from './components/price-approval/price-approval.component';
import { PromotionApprovalComponent } from './components/promotion-approval/promotion-approval.component';

export const routes: Routes = [
  { path: '', component: BranchSelectComponent },
  { path: 'branch/:branchId', component: BranchActionsComponent },
  { path: 'branch/:branchId/price-updates', component: PriceApprovalComponent },
  { path: 'branch/:branchId/promotions', component: PromotionApprovalComponent },
  { path: '**', redirectTo: '' }
];
