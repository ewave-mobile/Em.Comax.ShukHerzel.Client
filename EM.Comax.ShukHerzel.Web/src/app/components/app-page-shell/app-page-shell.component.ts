import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AppBrandComponent } from '../app-brand/app-brand.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-page-shell',
  imports: [AppBrandComponent, MatButtonModule, MatIconModule, MatTooltipModule],
  templateUrl: './app-page-shell.component.html',
  styleUrl: './app-page-shell.component.scss'
})
export class AppPageShellComponent {
  private readonly auth = inject(AuthService);

  readonly isAuthenticated = this.auth.isAuthenticated;
  readonly userName = this.auth.userName;

  logout(): void {
    this.auth.logout();
  }
}
