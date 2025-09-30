// core/auth/auth.state.ts
import { Injectable, inject } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  firstValueFrom,
  Observable,
  of,
  shareReplay,
  switchMap,
  tap,
} from 'rxjs';
import { AuthService } from './auth.service';
import { UserMeDto } from '../../Models/login.model';

@Injectable({ providedIn: 'root' })
export class AuthState {
  private authService = inject(AuthService);

  private readonly storageKey = 'me';
  private _me$ = new BehaviorSubject<UserMeDto | null>(null);
  readonly me$ = this._me$.asObservable();

  get current(): UserMeDto | null {
    return this._me$.value;
  }

  hydrateFromStorage(): void {
    try {
      const raw = sessionStorage.getItem(this.storageKey);
      if (raw) this._me$.next(JSON.parse(raw) as UserMeDto);
    } catch {
      /* ignore */
    }
  }

  loadMe(): Observable<UserMeDto | null> {
    return this.authService.GetMe().pipe(
      tap((me) => this.normalizeAndCache(me)),

      switchMap(() => of(this._me$.value)),
      shareReplay(1)
    );
  }

  loadMeOptional(): Observable<UserMeDto | null> {
    return this.authService.GetMeOptional().pipe(
      tap(me => this.normalizeAndCache(me)),
      switchMap(() => of(this._me$.value)),
      catchError(() => of(null)),     // 401 -> null, sin refresh ni redirect
      shareReplay(1)
    );
  }

  /** Forzar recarga desde el backend y re-cachear */
  reloadMe(): Observable<UserMeDto | null> {
    return this.authService.GetMe().pipe(
      tap((me) => this.normalizeAndCache(me)),
      catchError((_err) => {
        this.clear();
        return of(null);
      }),
      shareReplay(1)
    );
  }

  /** Versión async conveniente para await */
  async reloadMeOnce(): Promise<UserMeDto | null> {
    return await firstValueFrom(this.reloadMe());
  }

  private normalizeAndCache(me: UserMeDto) {
    // 1) Normaliza roles
    me.roles = (me.roles ?? [])
      .map((r) => (r ?? '').toString().trim())
      .filter(Boolean);

    // 2) Normaliza a minúsculas para cache (opcional pero consistente)
    //    Si haces esto, ajusta hasRole a comparar en minúsculas (ya lo haces).
    me.roles = me.roles.map((r) => r.toLowerCase());

    // 3) Normaliza permisos y forms (como ya tenías)
    me.permissions = (me.permissions ?? []).map((p) => p.toLowerCase());
    me.menu?.forEach((s) =>
      s.forms?.forEach(
        (f) =>
          (f.permissions = (f.permissions ?? []).map((p) => p.toLowerCase()))
      )
    );

    this._me$.next(me);
    sessionStorage.setItem(this.storageKey, JSON.stringify(me));
  }

  clear(): void {
    sessionStorage.removeItem(this.storageKey);
    this._me$.next(null);
  }



  hasRole(role: string): boolean {
    const me = this._me$.value;
    if (!me?.roles?.length) return false;
    const wanted = (role ?? '').toLowerCase();
    return me.roles.some((r) => (r ?? '').toLowerCase() === wanted);
  }

  hasFormPermission(routeKeyOrUrl: string, action: string): boolean {
    const me = this._me$.value;
    if (!me) return false;
    const key = (routeKeyOrUrl ?? '').toLowerCase();
    const form = me.menu
      ?.flatMap((m) => m.forms ?? [])
      .find((f) => (f.url ?? '').toLowerCase() === key);
    return !!form && (form.permissions ?? []).includes(action.toLowerCase());
  }

  getMenu() {
    return this._me$.value?.menu ?? [];
  }
}
