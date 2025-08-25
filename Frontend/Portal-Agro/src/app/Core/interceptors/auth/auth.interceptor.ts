// core/http/auth.interceptor.ts
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';
import { environment } from '../../../../environments/environment';

// Debe coincidir con _cookieSettings.CsrfCookieName del backend
const CSRF_COOKIE_NAME = 'XSRF-TOKEN';

function readCookie(name: string): string | null {
  const m = document.cookie.match(new RegExp('(?:^|; )' + name + '=([^;]*)'));
  return m ? decodeURIComponent(m[1]) : null;
}

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const apiBase = environment.apiUrl; 
  const isApi = req.url.startsWith(apiBase);
  const isRefresh = /\/Auth\/refresh$/i.test(req.url); // case-insensitive y ruta correcta

  if (isApi) {
    const csrf = readCookie(CSRF_COOKIE_NAME);
    req = req.clone({
      setHeaders: csrf ? { 'X-XSRF-TOKEN': csrf } : {}
    });
  }

  return next(req).pipe(
    catchError((error) => {
      const is401 = error instanceof HttpErrorResponse && error.status === 401;

      if (is401 && isApi && !isRefresh) {
        return auth.RefreshToken().pipe(
          // Reintenta el request original tal cual
          switchMap(() => next(req)),
          catchError((refreshErr) => {
            // Si el refresh falla (401/403), limpias estado y rediriges
            // OJO: si mantienes un store de usuario, límpialo aquí
            router.navigate(['/login']);
            return throwError(() => refreshErr);
          })
        );
      }

      return throwError(() => error);
    })
  );
};
