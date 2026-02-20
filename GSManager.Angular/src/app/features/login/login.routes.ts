import { Routes } from '@angular/router';

export const LOGIN_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./login').then(m => m.LoginComponent)
  }
];

export default LOGIN_ROUTES;
