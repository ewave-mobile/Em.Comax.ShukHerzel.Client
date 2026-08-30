import { Routes } from '@angular/router';
import { BranchSelectComponent } from './components/branch-select/branch-select.component';
import { BranchActionsComponent } from './components/branch-actions/branch-actions.component';
import { PriceApprovalComponent } from './components/price-approval/price-approval.component';
import { PromotionApprovalComponent } from './components/promotion-approval/promotion-approval.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: BranchSelectComponent, canActivate: [authGuard] },
  { path: 'branch/:branchId', component: BranchActionsComponent, canActivate: [authGuard] },
  { path: 'branch/:branchId/price-updates', component: PriceApprovalComponent, canActivate: [authGuard] },
  { path: 'branch/:branchId/promotions', component: PromotionApprovalComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
