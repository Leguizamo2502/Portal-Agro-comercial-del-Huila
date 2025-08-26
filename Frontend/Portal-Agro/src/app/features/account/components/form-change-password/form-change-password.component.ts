import { Component, inject, Input } from '@angular/core';
import { ButtonComponent } from "../../../../shared/components/button/button.component";
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from "@angular/material/input";
import { PasswordPolicyService } from '../../../../shared/services/passwordPolicy/password-policy.service';
import Swal from 'sweetalert2';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { ChangePasswordModel } from '../../../../Core/Models/changePassword.model';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-form-change-password',
  imports: [ButtonComponent, MatInputModule,ReactiveFormsModule,MatFormFieldModule,CommonModule],
  templateUrl: './form-change-password.component.html',
  styleUrl: './form-change-password.component.css'
})
export class FormChangePasswordComponent {
  private fb = inject(FormBuilder);
  private policy = inject(PasswordPolicyService);
  private auth = inject(AuthService);
  private router = inject(Router);

  title = 'Cambiar Contraseña';
  // submitted = false;

  form = this.fb.group({
    currentPassword: ['', Validators.required],
    newPassword: ['', [this.policy.validator()]],     // <— reutiliza servicio
    confirmNewPassword: ['', Validators.required]
  }, { validators: this.policy.passwordsMatch('newPassword', 'confirmNewPassword') });

  get f() { return this.form.controls; }

  save(): void {
    // this.submitted = true; 
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    if (this.f.currentPassword.value === this.f.newPassword.value) {
      Swal.fire({ icon: 'warning', title: 'La nueva contraseña no puede ser igual a la actual.' });
      return;
    }
    const objeto: ChangePasswordModel ={
      currentPassword: this.f.currentPassword.value!,
      newPassword: this.f.newPassword.value!
    }

    this.auth.ChangePassword(objeto).subscribe({
      next: () => { Swal.fire({ icon: 'success', title: 'Contraseña actualizada' }); this.form.reset(); 
      this.router.navigate(['/account/info']);},
      error: (err) => Swal.fire({ icon: 'error', title: 'Error', text: err?.error?.message ?? 'No se pudo cambiar la contraseña.' })
    });
  }
  
}
