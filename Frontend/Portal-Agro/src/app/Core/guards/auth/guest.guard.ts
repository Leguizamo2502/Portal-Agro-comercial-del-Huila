// Core/guards/auth.guards.ts
import { inject } from '@angular/core';
import { CanMatchFn, Router, UrlTree } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';

// Rutas privadas: requiere sesión
export const authGuard: CanMatchFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.GetMe().pipe(
    map(() => true),                                   // autenticado => pasa
    catchError(() => of(router.parseUrl('/auth/login')))    // error/401 => redirige (OBSERVABLE de UrlTree)
  );
};

// Rutas públicas (login/registro): bloquea si ya hay sesión
export const guestGuard: CanMatchFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.GetMe().pipe(
    map(() => router.parseUrl('/home')), // ya logueado => redirige (UrlTree)
    catchError(() => of(true))                // no logueado/401 => permite entrada (boolean)
  );
};
