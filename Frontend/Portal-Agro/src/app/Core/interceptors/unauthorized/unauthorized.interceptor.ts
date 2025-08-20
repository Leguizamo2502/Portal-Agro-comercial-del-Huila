import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthState } from '../../services/auth/auth.state';

export const unauthorizedInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authState = inject(AuthState);

  return next(req).pipe({
    error: (err: { status: number; }, _caught: any) => {
      if (err?.status === 401) {
        authState.clear();
        router.navigate(['/auth/login'], { queryParams: { redirectTo: location.pathname } });
      }
      throw err;
    }
  } as any);
};
