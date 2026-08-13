import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-page-header',
  imports: [MatButtonModule, MatIconModule, MatTooltipModule],
  templateUrl: './app-page-header.component.html',
  styleUrl: './app-page-header.component.css'
})
export class AppPageHeaderComponent {
  readonly title = input.required<string>();
  readonly subtitle = input<string | null>(null);
  readonly showBack = input(false);
  readonly showRefresh = input(false);
  readonly refreshing = input(false);

  readonly back = output<void>();
  readonly refresh = output<void>();
}
