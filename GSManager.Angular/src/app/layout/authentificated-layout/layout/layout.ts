import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthentificatedHeaderComponent } from '../header/header';
import { AuthentificatedSidebarComponent } from '../sidebar/sidebar';

@Component({
  selector: 'app-authentificated-layout',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterOutlet, AuthentificatedHeaderComponent, AuthentificatedSidebarComponent],
  styleUrl: './layout.scss',
  template: `
    <div class="layout">      
      <app-authentificated-header (menuToggle)="toggleSidebar()"/>
      <div class="layout-body">

        <app-authentificated-sidebar [collapsed]="sidebarCollapsed()"/>
        
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
export class AuthentificatedLayoutComponent {
  sidebarCollapsed = signal(false);

  toggleSidebar(): void {
    this.sidebarCollapsed.update(v => !v);
  }

}
