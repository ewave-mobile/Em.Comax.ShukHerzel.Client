import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MatRadioModule } from '@angular/material/radio';
import { BranchStoreService } from '../../services/branch-store.service';
import { AppPageShellComponent } from '../../shared/components/app-page-shell/app-page-shell.component';
import { AppPageHeaderComponent } from '../../shared/components/app-page-header/app-page-header.component';
import { PageStatusComponent } from '../../shared/components/page-status/page-status.component';
import { FooterActionComponent } from '../../shared/components/footer-action/footer-action.component';

@Component({
  selector: 'app-branch-select',
  imports: [
    MatRadioModule,
    AppPageShellComponent,
    AppPageHeaderComponent,
    PageStatusComponent,
    FooterActionComponent
  ],
  templateUrl: './branch-select.component.html',
  styleUrl: './branch-select.component.css'
})
export class BranchSelectComponent implements OnInit {
  private readonly store = inject(BranchStoreService);
  private readonly router = inject(Router);

  readonly branches = this.store.branches;
  readonly selectedId = signal<number | null>(null);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  readonly selectedBranch = computed(() => this.store.byId(this.selectedId()));

  readonly continueLabel = computed(() => {
    const branch = this.selectedBranch();
    return branch ? `כניסה לסניף ${branch.branchName}` : 'בחר סניף להמשך';
  });

  ngOnInit(): void {
    this.loading.set(true);
    this.error.set(null);
    this.store.load().subscribe({
      next: branches => {
        this.loading.set(false);
        if (branches.length === 1) {
          this.selectedId.set(branches[0].id);
        }
      },
      error: () => {
        this.loading.set(false);
        this.error.set('שגיאה בטעינת סניפים');
      }
    });
  }

  enterBranch(): void {
    const branchId = this.selectedId();
    if (branchId == null) {
      return;
    }
    this.router.navigate(['/branch', branchId]);
  }
}
