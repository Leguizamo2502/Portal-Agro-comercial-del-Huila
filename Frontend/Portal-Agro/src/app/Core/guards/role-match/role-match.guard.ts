// Core/guards/role-match/role-match.guard.ts
import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, CanMatchFn, Router, UrlTree } from '@angular/router';
import { of, map, catchError } from 'rxjs';
import { AuthState } from '../../services/auth/auth.state';

function checkAccess(rolesRequiredRaw: string[] | undefined, router: Router, auth: AuthState): boolean | UrlTree {
  const rolesRequired = (rolesRequiredRaw ?? []).map(r => (r ?? '').toLowerCase());
  if (rolesRequired.length === 0) return true;

  const wantProducer = rolesRequired.includes('producer');
  const hasRole = (name: string) => auth.hasRole?.(name) === true;

  const roles = auth.current?.roles ?? [];
  const isConsumer = roles.length === 0 || roles.some(r => (r ?? '').toLowerCase() === 'consumer');

  // Logs de depuración
  console.log('[RoleGuard] user roles =', roles, 'required =', rolesRequired);

  if (hasRole('Admin')) return true;
  if (rolesRequired.some(r => hasRole(r))) return true;

  if (wantProducer && isConsumer) {
    return router.parseUrl('/account/become-producer'); // ruta pública
  }
  return router.parseUrl('/forbidden');
}

function decideWithMe(rolesRequiredRaw: string[] | undefined, router: Router, auth: AuthState): boolean | UrlTree | any {
  if (auth.current) return checkAccess(rolesRequiredRaw, router, auth);
  return auth.loadMe().pipe(
    map(() => checkAccess(rolesRequiredRaw, router, auth)),
    catchError(() => of(router.parseUrl('/auth/login')))
  );
}

// 1) Guard para canMatch (rutas lazy en ACCOUNT_ROUTES)
export const roleMatchGuard: CanMatchFn = (route) => {
  const auth = inject(AuthState);
  const router = inject(Router);
  const rolesRequired = route.data?.['roles'] as string[] | undefined;
  return decideWithMe(rolesRequired, router, auth);
};

// 2) Guard para canActivate (raíz de PRODUCER_ROUTES)
export const roleActivateGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const auth = inject(AuthState);
  const router = inject(Router);
  const rolesRequired = route.data?.['roles'] as string[] | undefined;
  return decideWithMe(rolesRequired, router, auth);
};
