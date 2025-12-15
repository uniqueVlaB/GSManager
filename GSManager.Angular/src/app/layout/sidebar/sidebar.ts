import { Component, ChangeDetectionStrategy, input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, RouterLinkActive],
  styleUrl: './sidebar.scss',
  template: `
    <aside class="sidebar" [class.collapsed]="collapsed()">
      <nav class="sidebar-nav" aria-label="Main navigation">
        <ul class="nav-list">
          @for (item of navItems; track item.route) {
            <li>
              <a 
                [routerLink]="item.route"
                routerLinkActive="active"
                [routerLinkActiveOptions]="{ exact: item.route === '/dashboard' }"
                class="nav-link"
                [attr.aria-label]="collapsed() ? item.label : null">
                <span class="nav-icon">{{ item.icon }}</span>
                @if (!collapsed()) {
                  <span class="nav-label">{{ item.label }}</span>
                }
              </a>
            </li>
          }
        </ul>
      </nav>
      
      <div class="sidebar-footer">
        <a routerLink="/settings" class="nav-link" aria-label="Settings">
          <span class="nav-icon">⚙️</span>
          @if (!collapsed()) {
            <span class="nav-label">Settings</span>
          }
        </a>
      </div>
    </aside>
  `
})
export class SidebarComponent {
  collapsed = input<boolean>(false);

  navItems: NavItem[] = [
    { label: 'Dashboard', icon: '📊', route: '/dashboard' },
    { label: 'Members', icon: '👥', route: '/members' },
    { label: 'Plots', icon: '🌱', route: '/plots' },
    { label: 'Payments', icon: '💰', route: '/payments' },
    { label: 'Communications', icon: '📧', route: '/communications' },
    { label: 'Reports', icon: '📈', route: '/reports' },
    { label: 'Administration', icon: '🔧', route: '/admin' }
  ];
}
