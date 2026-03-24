import { Routes } from '@angular/router';

export const USER_ROUTES: Routes = [
  {
    path: 'profile',
    loadComponent: () => import('./profile/profile').then(m => m.ProfileComponent)
  }
];

export default USER_ROUTES;
