import { Component, ChangeDetectionStrategy, inject, computed } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/services';
import { AppPermission } from '../../shared/enums/app-permission.enum';

interface QuickLink {
  label: string;
  description: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-home',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  template: `
    <div class="home">
      <header class="home-header">
        <div>
          <h1 class="page-title">Welcome back@if (username()) {, {{ username() }}}!</h1>
          <p class="page-subtitle">Here's an overview of what you can do in Garden Society Manager.</p>
        </div>
      </header>

      <section class="quick-links" aria-label="Quick links">
        @for (link of visibleLinks(); track link.route) {
          <a [routerLink]="link.route" class="quick-link-card">
            <span class="quick-link-icon" aria-hidden="true">{{ link.icon }}</span>
            <div class="quick-link-body">
              <span class="quick-link-label">{{ link.label }}</span>
              <span class="quick-link-desc">{{ link.description }}</span>
            </div>
          </a>
        }
      </section>
    </div>
  `,
  styleUrl: './home.scss'
})
export class HomeComponent {
  private readonly authService = inject(AuthService);

  readonly username = this.authService.username;

  private readonly allLinks: (QuickLink & { permissions?: AppPermission[] })[] = [
    {
      label: 'Dashboard',
      description: 'View key statistics and summaries.',
      icon: '📊',
      route: '/dashboard'
    },
    {
      label: 'Members',
      description: 'Manage society members and their details.',
      icon: '👥',
      route: '/members',
      permissions: [AppPermission.ViewMembers]
    },
    {
      label: 'Plots',
      description: 'View and manage allotment plots.',
      icon: '🌱',
      route: '/plots',
      permissions: [AppPermission.ViewPlots]
    }
  ];

  readonly visibleLinks = computed(() =>
    this.allLinks.filter(link =>
      !link.permissions || this.authService.hasPermission(...link.permissions)
    )
  );
}
