import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  template: `
    <div class="not-found">
      <span class="not-found-icon" aria-hidden="true">🔍</span>
      <h1 class="not-found-title">Page Not Found</h1>
      <p class="not-found-message">
        The page you are looking for does not exist or has been moved.
      </p>
      <a routerLink="/dashboard" class="back-link">Return to Dashboard</a>
    </div>
  `,
  styles: [`
    .not-found {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      text-align: center;
      padding: 4rem 2rem;
      max-width: 480px;
      margin: 0 auto;
    }

    .not-found-icon {
      font-size: 4rem;
      margin-bottom: 1rem;
    }

    .not-found-title {
      font-size: 1.75rem;
      font-weight: 700;
      color: var(--text-color);
      margin: 0 0 0.5rem;
    }

    .not-found-message {
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
export class NotFoundComponent {}
