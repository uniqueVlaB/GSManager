import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-access-denied',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  template: `
    <div class="access-denied">
      <span class="access-denied-icon" aria-hidden="true">🚫</span>
      <h1 class="access-denied-title">Access Denied</h1>
      <p class="access-denied-message">
        You do not have the required permissions to view this page.
      </p>
      <a routerLink="/dashboard" class="back-link">Return to Dashboard</a>
    </div>
  `,
  styles: [`
    .access-denied {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      text-align: center;
      padding: 4rem 2rem;
      max-width: 480px;
      margin: 0 auto;
    }

    .access-denied-icon {
      font-size: 4rem;
      margin-bottom: 1rem;
    }

    .access-denied-title {
      font-size: 1.75rem;
      font-weight: 700;
      color: var(--text-color);
      margin: 0 0 0.5rem;
    }

    .access-denied-message {
      color: var(--text-muted);
      margin: 0 0 2rem;
      line-height: 1.5;
    }

    .back-link {
      display: inline-block;
      padding: 0.625rem 1.5rem;
      background: var(--primary-color);
      color: var(--text-white);
      border-radius: 8px;
      text-decoration: none;
      font-weight: 500;
      transition: background 0.2s;

      &:hover {
        background: var(--primary-hover);
      }

      &:focus-visible {
        outline: 2px solid var(--primary-color);
        outline-offset: 2px;
      }
    }
  `]
})
export class AccessDeniedComponent {}
