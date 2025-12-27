import { Component, ChangeDetectionStrategy, output, input, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ThemeService } from '../../core/services';

@Component({
  selector: 'app-header',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  styleUrl: './header.scss',
  template: `
    <header class="header">
      <div class="header-left">
        <button 
          class="menu-toggle" 
          (click)="menuToggle.emit()">
          <span class="menu-icon" [class.open]="isMenuOpen()"></span>
        </button>
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
          <span class="theme-icon">{{ themeService.theme() === 'light' ? '🌙' : '☀️' }}</span>
        </button>

        <button class="header-btn" aria-label="Notifications">
          <span class="notification-icon">🔔</span>
          <span class="notification-badge">3</span>
        </button>
        <div class="user-menu">
          <button class="user-btn" aria-label="User menu">
            <span class="user-avatar">JK</span>
            <span class="user-name">Jan Kowalski</span>
          </button>
        </div>
      </div>
    </header>
  `
})
export class HeaderComponent {
  themeService = inject(ThemeService);
  isMenuOpen = input<boolean>(false);
  menuToggle = output<void>();
}
