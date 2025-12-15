import { Routes } from '@angular/router';

export const MEMBER_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./member-list/member-list').then(m => m.MemberListComponent)
  }
];

export default MEMBER_ROUTES;
