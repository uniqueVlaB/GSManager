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
        path: 'login',
        canActivate: [noAuthGuard],
        loadChildren: () => import('./features/login/login.routes')
      },
      {
        path: 'plots',
        canActivate: [authGuard],
        loadChildren: () => import('./features/plots/plots.routes')
      },
      {
        path: 'members',
        canActivate: [authGuard, permissionGuard(AppPermission.ManageMembers)],
        loadChildren: () => import('./features/members/members.routes')
      },
      {
        path: 'access-denied',
        loadChildren: () => import('./features/state-pages/access-denied/access-denied.routes')
      }
    ]
  },

  // ── Wildcard ───────────────────────────────────────────────────────
  {
    path: '**',
    loadComponent: () => import('./features/state-pages/not-found').then(m => m.NotFoundComponent)
  }
];
