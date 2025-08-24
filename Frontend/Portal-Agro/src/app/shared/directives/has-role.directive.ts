// shared/directives/has-role.directive.ts
import { Directive, Input, OnDestroy, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthState } from '../../Core/services/auth/auth.state';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnDestroy {
  private vcr = inject(ViewContainerRef);
  private tpl = inject(TemplateRef<any>);
  private auth = inject(AuthState);
  private sub?: Subscription;

  private rolesWanted: string[] = [];

  @Input() set appHasRole(roles: string | string[]) {
    this.rolesWanted = (Array.isArray(roles) ? roles : [roles]).map(r => (r ?? '').toLowerCase());
    this.bind();
  }

  private bind() {
    this.sub?.unsubscribe();
    this.sub = this.auth.me$.subscribe(() => {
      const has = this.rolesWanted.some(r => this.auth.hasRole(r));
      this.vcr.clear();
      if (has) this.vcr.createEmbeddedView(this.tpl);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }
}
