import {
  FormBuilder,
  FormGroup,



  ReactiveFormsModule,



  Validators,
} from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { RegisterUserModel } from '../../Models/registeruser.model';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LocationService } from '../../../../shared/services/location/location.service';
import {
  CityModel,
  DepartmentModel,
} from '../../../../shared/models/location/location.model';
import { CommonModule } from '@angular/common';

// Importaciones de Angular Material - AGREGAR ESTAS AL MODULE
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,

    CommonModule,
    // Imports de Angular Material - NECESARIOS PARA EL FUNCIONAMIENTO
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  public fb = inject(FormBuilder);
  private _servicio = inject(AuthService);
  private _router = inject(Router);
  private _location = inject(LocationService);

  departments: DepartmentModel[] = [];
  cities: CityModel[] = [];

  // PASO 1: Información personal - PUEDES CAMBIAR LOS VALIDATORS AQUÍ
  public firstFormGroup: FormGroup = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    identification: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
    phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
    address: ['', Validators.required],
    departmentId: ['', Validators.required],
    cityId: ['', Validators.required],
  });

  // PASO 2: Credenciales de acceso - PUEDES CAMBIAR LOS VALIDATORS AQUÍ
  public secondFormGroup: FormGroup = this.fb.group(
    {
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    },
    {
      // Validator personalizado para confirmar contraseña - PUEDES MODIFICAR LA LÓGICA AQUÍ
      validators: this.passwordMatchValidator,
    }
  );

  // Variable para controlar si el stepper es lineal - CAMBIA A false SI QUIERES NAVEGACIÓN LIBRE
  isLinear = true;

  ngOnInit(): void {
    this.loadDeparment();
    this.selectDepartment();
  }

  // Validator personalizado para contraseñas - PUEDES PERSONALIZAR LA VALIDACIÓN AQUÍ
  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');

    if (
      password &&
      confirmPassword &&
      password.value !== confirmPassword.value
    ) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }

    return null;
  }
  currentStep = 1;

  nextStep() {
    if (this.firstFormGroup.valid) {
      this.currentStep = 2;
    }
  }

  prevStep() {
    this.currentStep = 1;
  }
  selectDepartment() {
    this.firstFormGroup
      .get('departmentId')
      ?.valueChanges.subscribe((id: number) => {
        if (id) {
          this.loadCities(id);
          this.firstFormGroup.get('cityId')?.setValue('');
        } else {
          this.cities = [];
          this.firstFormGroup.get('cityId')?.setValue('');
        }
      });
  }

  loadDeparment() {
    this._location.getDepartment().subscribe((data) => {
      this.departments = data;
      console.log(data);
    });
  }

  loadCities(id: number) {
    this._location.getCity(id).subscribe((data) => {
      this.cities = data;
    });
  }

  // Método para obtener mensajes de error - PUEDES PERSONALIZAR LOS MENSAJES AQUÍ
  getErrorMessage(formGroup: FormGroup, fieldName: string): string {
    const field = formGroup.get(fieldName);

    if (field?.hasError('required')) {
      return 'Este campo es requerido';
    }
    if (field?.hasError('email')) {
      return 'Email no válido';
    }
    if (field?.hasError('minlength')) {
      return 'Mínimo 6 caracteres';
    }
    if (field?.hasError('pattern')) {
      return 'Solo números permitidos';
    }
    if (field?.hasError('passwordMismatch')) {
      return 'Las contraseñas no coinciden';
    }

    return '';
  }

  register() {
    // Validar ambos formularios antes de enviar - PUEDES AGREGAR MÁS VALIDACIONES AQUÍ
    if (this.firstFormGroup.invalid || this.secondFormGroup.invalid) {
      return;
    }

    // Combinar datos de ambos formularios - MODIFICA SEGÚN TU MODELO
    const objeto: RegisterUserModel = {
      firstName: this.firstFormGroup.value.firstName,
      lastName: this.firstFormGroup.value.lastName,
      identification: this.firstFormGroup.value.identification,
      phoneNumber: this.firstFormGroup.value.phoneNumber,
      address: this.firstFormGroup.value.address,
      cityId: this.firstFormGroup.value.cityId,
      email: this.secondFormGroup.value.email,
      password: this.secondFormGroup.value.password,
    };

    this._servicio.Register(objeto).subscribe({
      next: (data) => {
        if (data.isSuccess) {
          // PUEDES CAMBIAR LA NAVEGACIÓN AQUÍ
          // this._router.navigate([""])
          Swal.fire({
            icon: 'success',
            title: 'Usuario Creado!',
            text: 'Usuario Creado Exitosamente!',
          });
          this._router.navigate(['/Auth/login'])
        } else {
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Error al Crear Usuario!',
          });
        }
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
}
