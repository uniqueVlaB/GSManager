import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../header/header';
import { SidebarComponent } from '../sidebar/sidebar';

@Component({
  selector: 'app-main-layout',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterOutlet, HeaderComponent, SidebarComponent],
  styleUrl: './main-layout.scss',
  template: `
    <div class="layout">
      <app-header [isMenuOpen]="!sidebarCollapsed()" (menuToggle)="toggleSidebar()" />
      
      <div class="layout-body">
        <app-sidebar [collapsed]="sidebarCollapsed()" />
        
        <main class="main-content" id="main-content">
          <router-outlet />
        </main>
      </div>
    </div>
    
    @if (!sidebarCollapsed()) {
      <div 
        class="sidebar-overlay" 
        (click)="toggleSidebar()"
        aria-hidden="true">
      </div>
    }
  `
})
export class MainLayoutComponent {
  sidebarCollapsed = signal(false);

  toggleSidebar(): void {
    this.sidebarCollapsed.update(v => !v);
  }
}
