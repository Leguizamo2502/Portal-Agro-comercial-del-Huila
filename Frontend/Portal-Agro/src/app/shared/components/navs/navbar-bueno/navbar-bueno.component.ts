import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { SidebarService } from '../../../services/sidebar/sidebar.service';
import { IfLoggedInDirective } from "../../../../Core/directives/if-logged-in.directive";
import { ButtonComponent } from "../../button/button.component";
import { IfLoggedOutDirective } from "../../../../Core/directives/if-logged-out.directive";
import { AuthState } from '../../../../Core/services/auth/auth.state';

@Component({
  selector: 'app-navbar-bueno',
  standalone: true,
  imports: [RouterLink, MatIcon, CommonModule, IfLoggedInDirective, ButtonComponent, IfLoggedOutDirective],
  templateUrl: './navbar-bueno.component.html',
  styleUrls: ['./navbar-bueno.component.css']
})
export class NavbarBuenoComponent {
  authService = inject(AuthService);
  ath = inject(AuthState);
  router = inject(Router);
  sidebarService = inject(SidebarService);

  get isAccountRoute(): boolean {
    return this.router.url.startsWith('/account');
  }

  toggleSidebar() {
    this.sidebarService.toggle();
  }

  logOut(): void {
    this.authService.LogOut().subscribe({
      next: () => {
        this.ath.clear();
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
      complete: () => console.log('Logout completo'),
    });
  }
}
