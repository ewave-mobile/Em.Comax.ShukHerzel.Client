import { Component } from '@angular/core';
import { AppBrandComponent } from '../app-brand/app-brand.component';

@Component({
  selector: 'app-page-shell',
  imports: [AppBrandComponent],
  templateUrl: './app-page-shell.component.html',
  styleUrl: './app-page-shell.component.scss'
})
export class AppPageShellComponent {}
