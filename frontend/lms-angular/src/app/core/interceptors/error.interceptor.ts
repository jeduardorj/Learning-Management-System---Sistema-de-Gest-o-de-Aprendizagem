import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        const rt = auth.getRefreshToken();
        if (rt) {
          return auth.refreshToken().pipe(
            switchMap(r => next(req.clone({ setHeaders: { Authorization: `Bearer ${r.accessToken}` } }))),
            catchError(() => { auth.logout(); return throwError(() => error); })
          );
        }
        auth.logout();
      }
      if (error.status === 403) router.navigate(['/dashboard']);
      return throwError(() => error);
    })
  );
};
