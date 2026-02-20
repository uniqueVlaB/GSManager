import { Routes } from '@angular/router';

export const HOME_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./home').then(m => m.HomeComponent)
  }
];

export default HOME_ROUTES;
