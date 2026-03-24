import { Routes } from '@angular/router';

export const AUTH_ROUTES: Routes = [
  {
    path: 'confirm-email',
    loadComponent: () => import('./confirm-email/confirm-email').then(m => m.ConfirmEmailComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./login/login').then(m => m.LoginComponent)
  }
];

export default AUTH_ROUTES;
