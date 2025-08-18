import { inject } from '@angular/core';
import { CanMatchFn, Router, UrlSegment, UrlTree } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { AuthState } from '../../services/auth/auth.state';


function isPublicProducerPath(segments: UrlSegment[]) {
  // rutas del módulo producer que deben ser públicas (sin rol) 
  const subpath = segments.map(s => s.path).join('/').toLowerCase();
  // Ej: '', 'become', 'onboarding'
  return subpath === '' || subpath.startsWith('become') || subpath.startsWith('onboarding');
}
export const producerMatchGuard: CanMatchFn = (route): boolean | UrlTree | any => {
  const auth   = inject(AuthState);
  const router = inject(Router);

  const roles = (route.data?.['roles'] as string[] | undefined) ?? [];
  const redirectIfDenied = route.data?.['redirectIfDenied'] as string | undefined;

  if (roles.length === 0) return true;

  const pass = roles.some(r => auth.hasRole(r));
  if (pass) return true;

  return auth.loadMe().pipe(
    map(() => (roles.some(r => auth.hasRole(r))
      ? true
      : router.parseUrl(redirectIfDenied ?? '/forbidden'))),
    catchError(() => of(router.parseUrl('/auth/login')))
  );
};
