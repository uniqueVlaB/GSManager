import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AppRole } from '../../shared/enums/app-roles.enum';

/**
 * Blocks unauthenticated users from accessing protected routes.
 * On a fresh page load it first tries to restore the session via the
 * refresh-token cookie before deciding.
 * Redirects to `/login`.
 */
export const authGuard: CanActivateFn = async () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    await authService.tryRestoreSession();
  }

  if (authService.isAuthenticated()) {
    return true;
  }

  return router.createUrlTree(['/login']);
};

export const roleGuard = (role: AppRole): CanActivateFn => () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  return authService.userRole() === role
    ? true
    : router.createUrlTree(['/access-denied']);
};

/**
 * Blocks authenticated users from accessing guest-only pages (login, register, etc.).
 * On a fresh page load it first tries to restore the session via the
 * refresh-token cookie before deciding.
 * Redirects to `/`.
 */
export const noAuthGuard: CanActivateFn = async () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    await authService.tryRestoreSession();
  }

  if (!authService.isAuthenticated()) {
    return true;
  }

  return router.createUrlTree(['/']);
};
