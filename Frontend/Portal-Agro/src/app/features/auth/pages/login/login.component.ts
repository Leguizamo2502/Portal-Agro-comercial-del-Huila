import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { RegisterUserModel } from '../../Models/registeruser.model';
import { LoginModel } from '../../Models/login.model';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule,RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  public fb = inject(FormBuilder);
  private _servicio = inject(AuthService);
  private _router = inject(Router);


  public formLogin : FormGroup =   this.fb.group({
    email:          ['',[Validators.required, Validators.email]],
    password:       ['', [Validators.required]],
  })

  // Helper method para mostrar errores
  getErrorMessage(field: string): string {
    const control = this.formLogin.get(field);
    if (control?.hasError('required')) {
      return `${field === 'email' ? 'Correo electrónico' : 'Contraseña'} es requerido`;
    }
    if (control?.hasError('email')) {
      return 'Ingrese un correo electrónico válido';
    }
    if (control?.hasError('minlength')) {
      return 'La contraseña debe tener al menos 6 caracteres';
    }
    return '';
  }


  me(){
    this._servicio.GetMe().subscribe({
      next: (data) => {
        console.log(data);
      }, error(err) {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: err.message,
        })
      }
    })
  }

  login(){
    if (this.formLogin.invalid) return;

    const objeto: LoginModel = {
      email: this.formLogin.value.email,
      password: this.formLogin.value.password,
    }
    this._servicio.Login(objeto).subscribe({
      next: (data) => {
        if (data != null) {
          this._router.navigate(["/home/inicio"])
          Swal.fire({
            icon: "success",
            title: "Exito",
            text: "Inicio de Sesión Exitoso!",
          })
        } else {
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Error create User!",
          })
        }
      }, error(err) {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: err.message,
        })
      }
    })


  }

}
