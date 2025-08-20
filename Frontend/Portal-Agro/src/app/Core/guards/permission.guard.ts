import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map, catchError, of } from 'rxjs';
import { AuthState } from '../services/auth/auth.state';

type Action = 'leer' | 'crear' | 'actualizar' | 'eliminar';
const norm = (u: string) => u.split('?')[0].replace(/\/+$/, '').toLowerCase();

export const permissionGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthState);
  const router = inject(Router);

  const required: Action = (route.data?.['perm'] as Action) ?? 'leer';
  const explicitKey = route.data?.['permissionKey'] as string | undefined;

  const url = norm(state.url); // <-- URL absoluta

  const me$ = auth.current ? of(auth.current) : auth.loadMe();

  return me$.pipe(
    map(me => {
      if (!me) {
        return router.createUrlTree(['/auth/login'], { queryParams: { redirectTo: state.url } });
      }

      // Bypass Admin
      if (me.roles?.some(r => r.toLowerCase() === 'admin')) return true;

      const forms = me.menu?.flatMap(s => s.forms ?? []) ?? [];
      const target = norm(explicitKey ?? url);

      // Acepta exacto o prefijo (para /create, /update/:id)
      const ok = forms.some(f => {
        const base = norm(f.url ?? '');
        if (!base) return false;

        const matchBase = target === base || target.startsWith(base + '/');

        const perms = (f.permissions ?? []).map((p: string) => p.toLowerCase());
        const has = perms.includes(required);
        return matchBase && has;
      });

      // LOG opcional
      console.log('[PermGuard] url=', url, 'req=', required, 'allow=', ok);

      return ok ? true : router.createUrlTree(['/forbidden']);
    }),
    catchError(async () => router.createUrlTree(['/auth/login'], { queryParams: { redirectTo: state.url } }))
  );
};
