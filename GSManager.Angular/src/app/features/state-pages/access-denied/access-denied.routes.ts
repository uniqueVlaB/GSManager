import { Routes } from '@angular/router';

export const ACCESS_DENIED_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./access-denied').then(m => m.AccessDeniedComponent)
  }
];

export default ACCESS_DENIED_ROUTES;
