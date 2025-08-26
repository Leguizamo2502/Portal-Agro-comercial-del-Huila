import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { take, finalize } from 'rxjs';
import Swal from 'sweetalert2';
import { RecoverPasswordModel, RecoverPasswordConfirmModel } from '../../../../Core/Models/changePassword.model';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { PasswordPolicyService } from '../../../../shared/services/passwordPolicy/password-policy.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-recover-password',
  imports: [ReactiveFormsModule,
    CommonModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,],
  templateUrl: './recover-password.component.html',
  styleUrl: './recover-password.component.css'
})
export class RecoverPasswordComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private auth = inject(AuthService);
  private pwdPolicy = inject(PasswordPolicyService);

  loading = signal(false);
  step = signal<1 | 2>(1);

  // Form principal con 2 grupos: step1 y step2
  form: FormGroup = this.fb.group({
    step1: this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    }),
    step2: this.fb.group(
      {
        emailConfirm: ['', [Validators.required, Validators.email]],
        code: ['', [Validators.required, Validators.minLength(4)]],
        newPassword: ['', [this.pwdPolicy.validator()]],
        confirmNewPassword: ['', [Validators.required]],
      },
      { validators: [this.pwdPolicy.passwordsMatch('newPassword', 'confirmNewPassword')] }
    ),
  });

  // Atajos a controles
  s1 = this.form.get('step1') as FormGroup;
  s2 = this.form.get('step2') as FormGroup;

  sameEmail = computed(() => {
    const e1 = (this.s1.get('email')?.value || '').trim().toLowerCase();
    const e2 = (this.s2.get('emailConfirm')?.value || '').trim().toLowerCase();
    return !!e1 && !!e2 && e1 === e2;
  });

  ngOnInit(): void {}

  // Mensajes de error centralizados
  getErrorMessage(group: 'step1' | 'step2', field: string): string {
    const fg = group === 'step1' ? this.s1 : this.s2;
    const control = fg.get(field);
    if (!control) return '';

    if (control.hasError('required')) return 'Campo requerido';
    if (control.hasError('email')) return 'Email no válido';
    if (control.hasError('minlength')) return 'Longitud mínima no cumplida';
    if (control.hasError('passwordPolicy')) return 'La contraseña debe tener al menos 6 caracteres y una mayúscula';
    if (fg.hasError('passwordMismatch') && (field === 'confirmNewPassword' || field === 'newPassword')) {
      return 'Las contraseñas no coinciden';
    }
    return '';
  }

  // PASO 1: enviar código
  sendCode(): void {
    if (this.loading() || this.s1.invalid) {
      this.s1.markAllAsTouched();
      return;
    }

    const payload: RecoverPasswordModel = { email: this.s1.value.email };

    this.loading.set(true);
    Swal.fire({
      title: 'Enviando código...',
      text: 'Por favor espera',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    this.auth
      .RequestRecoverPassword(payload)
      .pipe(take(1), finalize(() => this.loading.set(false)))
      .subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Código enviado',
            text: 'Revisa tu correo e ingrésalo en el siguiente paso.',
          });
          // Prellenar emailConfirm con el mismo correo del paso 1
          this.s2.get('emailConfirm')?.setValue(this.s1.value.email);
          this.step.set(2);
        },
        error: (err) => {
          const msg = err?.error?.message || 'No se pudo enviar el código.';
          Swal.fire({ icon: 'error', title: 'Error', text: msg });
        },
      });
  }

  // PASO 2: confirmar con código y nueva contraseña
  confirm(): void {
    if (this.loading() || this.s2.invalid) {
      this.s2.markAllAsTouched();
      return;
    }

    // Validación adicional: emails deben coincidir
    const email1 = (this.s1.value.email || '').trim().toLowerCase();
    const email2 = (this.s2.value.emailConfirm || '').trim().toLowerCase();
    if (email1 !== email2) {
      Swal.fire({
        icon: 'error',
        title: 'Correos distintos',
        text: 'El correo del Paso 1 debe coincidir con el del Paso 2.',
      });
      return;
    }

    const newPwd: string = this.s2.value.newPassword;
    if (!this.pwdPolicy.isValid(newPwd)) {
      Swal.fire({
        icon: 'error',
        title: 'Contraseña inválida',
        text: 'Debe tener al menos 6 caracteres y una mayúscula.',
      });
      return;
    }

    const payload: RecoverPasswordConfirmModel = {
      email: email2,
      code: this.s2.value.code,
      newPassword: newPwd,
    };

    this.loading.set(true);
    Swal.fire({
      title: 'Confirmando...',
      text: 'Por favor espera',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    this.auth
      .ConfirmRecoverPassword(payload)
      .pipe(take(1), finalize(() => this.loading.set(false)))
      .subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Contraseña actualizada',
            text: 'Ahora puedes iniciar sesión con tu nueva contraseña.',
          });
          this.router.navigateByUrl('/auth/login');
        },
        error: (err) => {
          const msg = err?.error?.message || 'No se pudo actualizar la contraseña.';
          Swal.fire({ icon: 'error', title: 'Error', text: msg });
        },
      });
  }

  goBackToStep1(): void {
    this.step.set(1);
  }
}