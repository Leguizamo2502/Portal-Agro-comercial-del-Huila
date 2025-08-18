import { inject } from '@angular/core';
import { CanMatchFn, Router, UrlTree } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { AuthState } from '../../services/auth/auth.state';

export const roleMatchGuard: CanMatchFn = (route): boolean | UrlTree | any => {
  const auth   = inject(AuthState);
  const router = inject(Router);

  // Roles requeridos en la ruta lazy
  const roles = (route.data?.['roles'] as string[] | undefined) ?? [];

  // Si no se especifican roles, no bloquea la carga del módulo
  if (roles.length === 0) return true;

  // Atajo: si ya hay contexto y cumple, permite pasar
  const okNow = roles.some(r => auth.hasRole(r));
  if (okNow) return true;

  // Si no hay contexto o no cumple aún, intenta cargar /Auth/me y revalida
  return auth.loadMe().pipe(
    map(() => (roles.some(r => auth.hasRole(r)) ? true : router.parseUrl('/forbidden'))),
    // Si /me falla (no hay sesión/cookie), manda a login
    catchError(() => of(router.parseUrl('/auth/login')))
  );
};
