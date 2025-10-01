import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { RegisterUserModel } from '../../../../Core/Models/registeruser.model';
import { LoginModel } from '../../../../Core/Models/login.model';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { AuthState } from '../../../../Core/services/auth/auth.state';
import { finalize, switchMap, take } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  ngOnInit(): void {
    console.log('holalogin');
  }
  public fb = inject(FormBuilder);
  private _servicio = inject(AuthService);
  private _router = inject(Router);
  private _authState = inject(AuthState);

  loading = false;

  public formLogin: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  // Helper method para mostrar errores
  getErrorMessage(field: string): string {
    const control = this.formLogin.get(field);
    if (control?.hasError('required')) {
      return `${
        field === 'email' ? 'Correo electrónico' : 'Contraseña'
      } es requerido`;
    }
    if (control?.hasError('email')) {
      return 'Ingrese un correo electrónico válido';
    }
    if (control?.hasError('minlength')) {
      return 'La contraseña debe tener al menos 6 caracteres';
    }
    return '';
  }

  me() {
    this._servicio.GetMe().subscribe({
      next: (data) => {
        console.log(data);
      },
      error(err) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: err.message,
        });
      },
    });
  }

  login() {
    if (this.formLogin.invalid || this.loading) return;

    const objeto: LoginModel = {
      email: this.formLogin.value.email!,
      password: this.formLogin.value.password!,
    };

    this.loading = true;

    // Muestra alerta con spinner
    Swal.fire({
      title: 'Iniciando sesión...',
      text: 'Por favor espera',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      },
    });

    this._servicio
      .Login(objeto)
      .pipe(
        take(1),
        switchMap(() => this._authState.loadMe()),
        finalize(() => (this.loading = false))
      )
      .subscribe({
        next: (me) => {
          if (!me) {
            Swal.fire({
              icon: 'error',
              title: 'No se pudo cargar tu sesión',
              text: 'Intenta nuevamente.',
            });
            return;
          }
          this._router.navigateByUrl('/home');
          Swal.fire({
            icon: 'success',
            title: 'Éxito',
            text: 'Inicio de sesión exitoso.',
          });
        },
        error: (err) => {
          const msg =
            err?.status === 401
              ? 'Credenciales inválidas.'
              : err?.error?.message || 'No se pudo iniciar sesión.';
          Swal.fire({ icon: 'error', title: 'Oops...', text: msg });
        },
      });
  }
}
