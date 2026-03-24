import { Routes } from '@angular/router';
import { AppLayoutComponent } from './layout/app-layout/app-layout';
import { authGuard, noAuthGuard, permissionGuard } from './core/guards';
import { AppPermission } from './shared/enums/app-permission.enum';

export const routes: Routes = [
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./features/home/home.routes')
      },
      {
        path: 'plots',
        canActivate: [authGuard, permissionGuard(AppPermission.ViewPlots)],
        loadChildren: () => import('./features/plots/plots.routes')
      },
      {
        path: 'members',
        canActivate: [authGuard, permissionGuard(AppPermission.ViewMembers)],
        loadChildren: () => import('./features/members/members.routes')
      },
      {
        path: 'access-denied',
        loadChildren: () => import('./features/state-pages/access-denied/access-denied.routes')
      },
      {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.routes')
      },
      {
        path: 'user',
        canActivate: [authGuard],
        loadChildren: () => import('./features/user/user.routes')
      }
    ]
  },

  // ── Wildcard ───────────────────────────────────────────────────────
  {
    path: '**',
    loadComponent: () => import('./features/state-pages/not-found').then(m => m.NotFoundComponent)
  }
];
