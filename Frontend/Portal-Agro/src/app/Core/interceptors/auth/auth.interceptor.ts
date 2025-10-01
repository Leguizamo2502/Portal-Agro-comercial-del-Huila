// core/http/auth.interceptor.ts
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';
import { environment } from '../../../../environments/environment';
import { AuthState } from '../../services/auth/auth.state';
import { OPTIONAL_AUTH } from './auth-optional.token';



function getCookie(name: string): string | null {
  const m = document.cookie.match(new RegExp('(?:^|; )' + name + '=([^;]*)'));
  return m ? decodeURIComponent(m[1]) : null;
}



export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const userStore = inject(AuthState);
  const router = inject(Router);

  const isApiRequest = req.url.startsWith(environment.apiUrl);
  const isRefreshEndpoint = /\/auth\/refresh$/i.test(req.url);

   const isOptional = req.context.get(OPTIONAL_AUTH) === true;

  if (isApiRequest) {
    const csrfCookie = getCookie('XSRF-TOKEN');

    req = req.clone({
      withCredentials: true,
      setHeaders: csrfCookie ? { 'X-XSRF-TOKEN': csrfCookie } : {}
    });
  }

  return next(req).pipe(
    catchError((error) => {
      if (
        error instanceof HttpErrorResponse &&
        error.status === 401 &&
        isApiRequest &&
        !isRefreshEndpoint
      ) {
      
        if (isOptional) {
          return throwError(() => error);
        }

        // ⬇️ Flujo normal: intenta refresh y reintenta la original
        return authService.RefreshToken().pipe(
          switchMap(() => next(req)),
          catchError((refreshError) => {
            userStore.clear();
            router.navigate(['/']);
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => error);
    })
  );
};