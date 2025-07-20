import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Component, inject } from '@angular/core';
import { RegisterUserModel } from '../../Models/registeruser.model';
import { AuthService } from '../../services/auth.service';
import Swal from 'sweetalert2';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  public fb = inject(FormBuilder);
  private _servicio = inject(AuthService);
  private _router = inject(Router);


  public formRegister : FormGroup =   this.fb.group({
    email:          ['',[Validators.required, Validators.email]],
    password:       ['', [Validators.required, Validators.minLength(6)]],
    firstName:      ['',Validators.required],
    lastName:       ['',Validators.required],
    identification: ['',[Validators.required, Validators.pattern('^[0-9]+$')]],
    phoneNumber:    ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
    address:        ['', Validators.required],
    cityId:         ['',Validators.required]
  })

  register(){
    if (this.formRegister.invalid) return;

    const objeto: RegisterUserModel = {
      firstName: this.formRegister.value.firstName,
      lastName: this.formRegister.value.lastName,
      identification: this.formRegister.value.identification,
      phoneNumber: this.formRegister.value.phoneNumber,
      address: this.formRegister.value.address,
      cityId: this.formRegister.value.cityId,
      email: this.formRegister.value.email,
      password: this.formRegister.value.password,
    }
    this._servicio.Register(objeto).subscribe({
      next: (data) => {
        if (data.isSuccess) {
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
