import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { AuthState } from '../services/auth/auth.state';

type Action = 'leer' | 'crear' | 'actualizar' | 'eliminar';

const norm = (u: string) => u.split('?')[0].replace(/\/+$/, '').toLowerCase();

/** Lee route.data[key] y si no existe, sube por la jerarquía de rutas hasta encontrarlo */
const readDataUp = (route: ActivatedRouteSnapshot, key: string): any => {
  let r: ActivatedRouteSnapshot | null = route;
  while (r) {
    if (r.data && r.data[key] !== undefined) return r.data[key];
    r = r.parent!;
  }
  return undefined;
};

export const permissionGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthState);
  const router = inject(Router);

  // Permite declararlo en el hijo; si no existe, lo toma del padre (útil con canActivateChild)
  const required: Action = (readDataUp(route, 'perm') as Action) ?? 'leer';
  const explicitKey = readDataUp(route, 'permissionKey') as string | undefined;

  const url = norm(state.url); // URL absoluta del navegador
  const me$ = auth.current ? of(auth.current) : auth.loadMe();

  return me$.pipe(
    map(me => {
      if (!me) {
        return router.createUrlTree(['/auth/login'], { queryParams: { redirectTo: state.url } });
      }

      // Bypass Admin
      if (me.roles?.some(r => r.toLowerCase() === 'admin')) return true;

      const forms = me.menu?.flatMap(s => s.forms ?? []) ?? [];
      const target = norm(explicitKey ?? url); // usa la key heredada o la URL real

      const ok = forms.some(f => {
        const base = norm(f.url ?? '');
        if (!base) return false;

        // match exacto o por prefijo (para /create, /update/:id, etc.)
        const matchBase = target === base || target.startsWith(base + '/');

        const perms = (f.permissions ?? []).map((p: string) => p.toLowerCase());
        return matchBase && perms.includes(required);
      });

      if (!ok) return router.createUrlTree(['/forbidden']);
      return true;
    }),
    // Debe devolver Observable, no Promise
    catchError(() => of(router.createUrlTree(['/auth/login'], { queryParams: { redirectTo: state.url } })))
  );
};
