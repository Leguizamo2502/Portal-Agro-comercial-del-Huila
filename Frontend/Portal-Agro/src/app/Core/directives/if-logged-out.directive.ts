import { DestroyRef, Directive, inject, TemplateRef, ViewContainerRef } from '@angular/core';
import { startWith } from 'rxjs';
import { AuthState } from '../services/auth/auth.state';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Directive({
  selector: '[appIfLoggedOut]',
  standalone: true,
})
export class IfLoggedOutDirective {
   private tpl = inject(TemplateRef<any>);
  private vcr = inject(ViewContainerRef);
  private auth = inject(AuthState);
  private destroyRef = inject(DestroyRef);  // <-- inyecta DestroyRef
  private rendered = false;

  constructor() {
    this.auth.me$
      .pipe(
        startWith(this.auth.current),
        takeUntilDestroyed(this.destroyRef) // <-- pásalo aquí
      )
      .subscribe(me => {
        const shouldShow = !me;
        if (shouldShow && !this.rendered) {
          this.vcr.clear();
          this.vcr.createEmbeddedView(this.tpl);
          this.rendered = true;
        } else if (!shouldShow && this.rendered) {
          this.vcr.clear();
          this.rendered = false;
        }
      });
  }
}
