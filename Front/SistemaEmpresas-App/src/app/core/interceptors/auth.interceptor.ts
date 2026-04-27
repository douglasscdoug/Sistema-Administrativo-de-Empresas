import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {

      const isAuthRoute =
        req.url.includes('/auth/login') ||
        req.url.includes('/auth/refresh');
      
      if (err.status === 401 && !isAuthRoute) {
        return authService.refreshToken().pipe(
          switchMap((response) => {

            authService.setToken(response.token);
            authService.setRefreshToken(
              response.refreshToken
            );

            const retryRequest = req.clone({
              setHeaders: {
                Authorization: `Bearer ${response.token}`
              }
            });

            return next(retryRequest);
          }),
          catchError((refreshError) => {
            authService.logout();
            router.navigate(['/login']);

            return throwError(() => refreshError);
          })
        );
      }
      
      return throwError(() => err);
    })
  );
};
