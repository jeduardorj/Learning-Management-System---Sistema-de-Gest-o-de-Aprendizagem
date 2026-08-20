import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent) },
  { path: 'certificates/validate/:code', loadComponent: () => import('./features/certificates/certificate-validate.component').then(m => m.CertificateValidateComponent) },
  { path: 'dashboard', canActivate: [authGuard], loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
  { path: 'courses', canActivate: [authGuard], loadComponent: () => import('./features/courses/courses.component').then(m => m.CoursesComponent) },
  { path: 'courses/new', canActivate: [authGuard, adminGuard], loadComponent: () => import('./features/courses/course-form/course-form.component').then(m => m.CourseFormComponent) },
  { path: 'courses/:id/edit', canActivate: [authGuard, adminGuard], loadComponent: () => import('./features/courses/course-form/course-form.component').then(m => m.CourseFormComponent) },
  { path: 'courses/:id', canActivate: [authGuard], loadComponent: () => import('./features/courses/course-detail/course-detail.component').then(m => m.CourseDetailComponent) },
  { path: 'my-courses', canActivate: [authGuard], loadComponent: () => import('./features/my-courses/my-courses.component').then(m => m.MyCoursesComponent) },
  { path: 'certificates/:courseId', canActivate: [authGuard], loadComponent: () => import('./features/certificates/certificate.component').then(m => m.CertificateComponent) },
  { path: 'admin', canActivate: [authGuard, adminGuard], loadChildren: () => import('./features/admin/admin.routes').then(m => m.adminRoutes) },
  { path: '**', redirectTo: 'dashboard' }
];
