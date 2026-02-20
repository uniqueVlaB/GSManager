import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AppPermission } from '../../shared/enums/app-permission.enum';

/**
 * Creates a guard that checks whether the current user holds **any** of the
 * listed permissions.  Users with `full_access` always pass.
 *
 * Usage in routes:
 * ```ts
 * canActivate: [permissionGuard(AppPermission.ManageMembers)]
 * ```
 */
export function permissionGuard(...required: AppPermission[]): CanActivateFn {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (authService.hasPermission(...required)) {
      return true;
    }

    return router.createUrlTree(['/access-denied']);
  };
}
