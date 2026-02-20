import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { AuthService } from '../../core/services';
import { AnonimousLayoutComponent } from '../anonimous-layout/anonimous-layout';
import { AuthentificatedLayoutComponent } from '../authentificated-layout/layout/layout';

@Component({
  selector: 'app-layout',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [AnonimousLayoutComponent, AuthentificatedLayoutComponent],
  template: `
    @if (authService.isAuthenticated()) {
      <app-authentificated-layout />
    } @else {
      <app-anonimous-layout />
    }
  `
})
export class AppLayoutComponent {
  protected readonly authService = inject(AuthService);
}
