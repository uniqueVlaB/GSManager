import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="dashboard">
      <h1 class="page-title">Dashboard</h1>
      <p class="page-subtitle">Welcome to Garden Society Manager</p>
      
      <div class="placeholder-card">
        <span class="placeholder-icon">📊</span>
        <p>Dashboard content coming soon...</p>
      </div>
    </div>
  `,
  styles: [`
    .dashboard {
      max-width: 1400px;
      margin: 0 auto;
    }

    .page-title {
      font-size: 1.75rem;
      font-weight: 700;
      color: var(--text-color);
      margin: 0;
    }

    .page-subtitle {
      color: var(--text-muted);
      margin: 0.25rem 0 1.5rem;
    }

    .placeholder-card {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 4rem;
      background: var(--card-bg);
      border-radius: 12px;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      text-align: center;
      color: var(--text-muted);
      transition: background-color 0.3s;
    }

    .placeholder-icon {
      font-size: 4rem;
      margin-bottom: 1rem;
      opacity: 0.5;
    }
  `]
})
export class DashboardComponent {}
