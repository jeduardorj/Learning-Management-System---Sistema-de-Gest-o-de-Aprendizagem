import { Routes } from '@angular/router';
export const adminRoutes: Routes = [
  { path: '', loadComponent: () => import('./admin.component').then(m => m.AdminComponent) }
];
