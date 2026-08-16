import { Component, computed, input, output } from '@angular/core';
import { MatCheckboxModule } from '@angular/material/checkbox';

@Component({
  selector: 'app-selection-toolbar',
  imports: [MatCheckboxModule],
  templateUrl: './selection-toolbar.component.html',
  styleUrl: './selection-toolbar.component.scss'
})
export class SelectionToolbarComponent {
  readonly count = input(0);
  readonly selectedCount = input(0);
  readonly allSelected = input(false);
  readonly entityLabel = input('פריטים');

  readonly selectAll = output<boolean>();

  readonly partiallySelected = computed(
    () => this.selectedCount() > 0 && !this.allSelected()
  );
}
