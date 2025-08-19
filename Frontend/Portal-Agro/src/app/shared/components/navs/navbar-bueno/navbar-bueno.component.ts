import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { AuthService } from '../../../../Core/services/auth/auth.service';

@Component({
  selector: 'app-navbar-bueno',
  imports: [RouterLink, MatIcon,RouterLink],
  templateUrl: './navbar-bueno.component.html',
  styleUrl: './navbar-bueno.component.css',
})
export class NavbarBuenoComponent {
  authService = inject(AuthService);
  router = inject(Router);

  logOut(): void {
    this.authService.LogOut().subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Sesión cerrada',
          text: 'Has cerrado sesión correctamente',
          timer: 2000,
          showConfirmButton: false,
        });

        this.router.navigate(['auth/login']);
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Error al cerrar sesión',
          text: err?.message || 'Ocurrió un error inesperado',
        });
      },
      complete: () => {
        console.log('Logout completo');
      },
    });
  }
}
