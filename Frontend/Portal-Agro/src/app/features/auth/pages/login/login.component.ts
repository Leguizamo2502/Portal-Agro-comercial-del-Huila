import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { RegisterUserModel } from '../../Models/registeruser.model';
import { LoginModel } from '../../Models/login.model';
import { AuthService } from '../../../../Core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
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
          this._router.navigate([""])
          Swal.fire({
            icon: "success",
            title: "Oops...",
            text: "User Create!",
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
