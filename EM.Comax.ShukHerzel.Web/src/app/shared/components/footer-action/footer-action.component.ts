import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-footer-action',
  imports: [MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './footer-action.component.html',
  styleUrl: './footer-action.component.css'
})
export class FooterActionComponent {
  readonly label = input.required<string>();
  readonly loadingLabel = input('מבצע...');
  readonly disabled = input(false);
  readonly loading = input(false);

  readonly action = output<void>();
}
