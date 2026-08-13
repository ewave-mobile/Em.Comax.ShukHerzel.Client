import { Component, computed, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-approval-filters',
  imports: [
    MatButtonModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatNativeDateModule
  ],
  templateUrl: './approval-filters.component.html',
  styleUrl: './approval-filters.component.css'
})
export class ApprovalFiltersComponent {
  readonly search = input('');
  readonly date = input('');
  readonly searchPlaceholder = input('חיפוש');

  readonly searchChange = output<string>();
  readonly dateChange = output<string>();

  readonly selectedDate = computed(() => {
    const [year, month, day] = this.date().split('-').map(Number);
    return year && month && day ? new Date(year, month - 1, day) : null;
  });

  onDateChange(value: Date | null): void {
    if (!value || Number.isNaN(value.getTime())) {
      return;
    }

    const year = value.getFullYear();
    const month = String(value.getMonth() + 1).padStart(2, '0');
    const day = String(value.getDate()).padStart(2, '0');
    this.dateChange.emit(`${year}-${month}-${day}`);
  }
}
