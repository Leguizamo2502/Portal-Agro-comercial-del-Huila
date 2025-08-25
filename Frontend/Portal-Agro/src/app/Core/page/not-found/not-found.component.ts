import { Location } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonComponent } from "../../../shared/components/button/button.component";

@Component({
  selector: 'app-not-found',
  imports: [ButtonComponent],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css'
})
export class NotFoundComponent {
  private location = inject(Location);
  private router = inject(Router);

  goBack(): void {
    // Regresa a la página anterior si existe; si no, va al inicio
    if (window.history.length > 1) this.location.back();
    else this.router.navigateByUrl('/');
  }
}
