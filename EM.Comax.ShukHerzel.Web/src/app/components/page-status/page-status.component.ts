import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-page-status',
  imports: [MatIconModule, MatProgressSpinnerModule],
  templateUrl: './page-status.component.html',
  styleUrl: './page-status.component.scss'
})
export class PageStatusComponent {
  readonly error = input<string | null>(null);
  readonly success = input<string | null>(null);
  readonly loading = input(false);
  readonly empty = input(false);
  readonly noMatches = input(false);
  readonly loadingText = input('טוען...');
  readonly emptyText = input('אין נתונים להצגה');
  readonly noMatchesText = input('לא נמצאו תוצאות התואמות לסינון');
}
