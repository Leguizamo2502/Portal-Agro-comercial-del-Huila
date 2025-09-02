import {
  DestroyRef,
  Directive,
  inject,
  Input,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { startWith } from 'rxjs';
import { AuthState } from '../services/auth/auth.state';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Directive({
  selector: '[appIfLoggedIn]',
})
export class IfLoggedInDirective {
  private tpl = inject(TemplateRef<any>);
  private vcr = inject(ViewContainerRef);
  private auth = inject(AuthState);
  private destroyRef = inject(DestroyRef);

  // Flag interno para evitar renders innecesarios
  private rendered = false;

  // Opcional: invertir lógica con [appIfLoggedInInverted]="true" si lo necesitas en el futuro
  @Input('appIfLoggedInInverted') inverted = false;

  constructor() {
    this.auth.me$
      .pipe(startWith(this.auth.current), takeUntilDestroyed(this.destroyRef))
      .subscribe((me) => {
        const shouldShow = this.inverted ? !me : !!me;
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
