import { Component, ChangeDetectionStrategy, input, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services';
import { AppPermission } from '../../../shared/enums/app-permission.enum';

export interface NavItem {
  label: string;
  icon: string;
  route: string;
  /** Permissions required to see this item. Empty = visible to everyone. */
  permissions?: AppPermission[];
}

/** Master list of navigation items with their permission requirements. */
const ALL_NAV_ITEMS: NavItem[] = [
  { label: 'Home', icon: '🏠', route: '/' },
  { label: 'Members', icon: '👥', route: '/members', permissions: [ AppPermission.ManageMembers] },
  { label: 'Plots', icon: '🌱', route: '/plots', permissions: [ AppPermission.ManagePlots] },
  { label: 'Payments', icon: '💰', route: '/payments', permissions: [ AppPermission.ManagePayments] },
  { label: 'Communications', icon: '📧', route: '/communications', permissions: [AppPermission.FullAccess] },
  { label: 'Reports', icon: '📈', route: '/reports', permissions: [ AppPermission.ViewReports] },
  { label: 'Administration', icon: '🔧', route: '/admin', permissions: [AppPermission.FullAccess] }
];

@Component({
  selector: 'app-authentificated-sidebar',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, RouterLinkActive],
  styleUrl: './sidebar.scss',
  template: `
    <aside class="sidebar" [class.collapsed]="collapsed()">
      <nav class="sidebar-nav" aria-label="Main navigation">
        <ul class="nav-list">
          @for (item of visibleNavItems(); track item.route) {
            <li>
              <a 
                [routerLink]="item.route"
                routerLinkActive="active"
                [routerLinkActiveOptions]="{ exact: item.route === '/' }"
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
export class AuthentificatedSidebarComponent {
  private readonly authService = inject(AuthService);

  collapsed = input<boolean>(false);

  /** Filters nav items based on the user's permission claim. */
  visibleNavItems = computed<NavItem[]>(() => {
    return ALL_NAV_ITEMS.filter(item => {
      if (!item.permissions || item.permissions.length === 0) {
        return true;
      }
      return this.authService.hasPermission(...item.permissions);
    });
  });
}
