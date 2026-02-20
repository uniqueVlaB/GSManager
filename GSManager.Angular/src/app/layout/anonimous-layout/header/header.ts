import { Component, ChangeDetectionStrategy, output, input, inject} from '@angular/core';
import { RouterLink } from '@angular/router';
import { ThemeService } from '../../../core/services';
import { UserMenuComponent } from "./user-menu";

@Component({
  selector: 'app-anonimous-header',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, UserMenuComponent],
  styleUrl: './header.scss',
  template: `
    <header class="header">
      <div class="header-left">
        <a routerLink="/" class="logo">
          <span class="logo-icon">🌻</span>
          <span class="logo-text">Garden Society Manager</span>
        </a>
      </div>
      
      <div class="header-right">
        <button 
          class="header-btn theme-toggle" 
          (click)="themeService.toggleTheme()"
          [attr.aria-label]="themeService.theme() === 'light' ? 'Switch to dark mode' : 'Switch to light mode'">
          <span class="theme-icon">{{ themeService.theme() === 'light' ? '☀️' : '🌙' }}</span>
        </button>     
        <app-user-menu/>
      </div>
    </header>
  `
})
export class AnonimousHeaderComponent {
  themeService = inject(ThemeService);
  
  isMenuOpen = input<boolean>(false);
  menuToggle = output<void>();
}
