import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { RegisterUserModel } from '../../../../Core/Models/registeruser.model';
import Swal from 'sweetalert2';
import { Router, RouterLink } from '@angular/router';
import { LocationService } from '../../../../shared/services/location/location.service';
import {
  CityModel,
  DepartmentModel,
} from '../../../../shared/models/location/location.model';
import { CommonModule } from '@angular/common';

// Angular Material (si tu módulo los expone para standalone, mantenlos)
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { take, catchError, finalize } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [
    RouterLink,
    ReactiveFormsModule,
    CommonModule,
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
  standalone: true,
})
export class RegisterComponent implements OnInit {
  private fb = inject(FormBuilder);
  private _servicio = inject(AuthService);
  private _router = inject(Router);
  private _location = inject(LocationService);

  departments: DepartmentModel[] = [];
  cities: CityModel[] = [];

  // Paso 1: Credenciales
  public credentialsForm: FormGroup = this.fb.group(
    {
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    },
    { validators: this.passwordMatchValidator }
  );

  // Paso 2: Datos básicos
  public basicForm: FormGroup = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    identification: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
  });

  // Paso 3: Contacto y ubicación
  public contactForm: FormGroup = this.fb.group({
    phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
    address: ['', Validators.required],
    departmentId: ['', Validators.required],
    cityId: ['', Validators.required],
  });

  // Control del step actual
  currentStep = 1;
  isLinear = true;
  loading = false;

  ngOnInit(): void {
    this.loadDeparment();
    this.bindDepartmentWatcher();
  }

  // Validator de confirmación de contraseña
  private passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value ?? '';
    const confirm = form.get('confirmPassword')?.value ?? '';
    if (password && confirm && password !== confirm) {
      form.get('confirmPassword')?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    return null;
  }

  // Navegación entre pasos
  nextStep(): void {
    if (this.currentStep === 1 && this.credentialsForm.valid) {
      this.currentStep = 2;
      return;
    }
    if (this.currentStep === 2 && this.basicForm.valid) {
      this.currentStep = 3;
      return;
    }
  }

  prevStep(): void {
    if (this.currentStep > 1) this.currentStep -= 1;
  }

  // Carga y enlace de ubicaciones
  private bindDepartmentWatcher(): void {
    this.contactForm.get('departmentId')?.valueChanges.subscribe((id: number) => {
      if (id) {
        this.loadCities(id);
        this.contactForm.get('cityId')?.setValue('');
      } else {
        this.cities = [];
        this.contactForm.get('cityId')?.setValue('');
      }
    });
  }

  private loadDeparment(): void {
    this._location.getDepartment().subscribe((data) => {
      this.departments = data;
    });
  }

  private loadCities(id: number): void {
    this._location.getCity(id).subscribe((data) => {
      this.cities = data;
    });
  }

  // Mensajes de error reutilizables
  getErrorMessage(formGroup: FormGroup, fieldName: string): string {
    const field = formGroup.get(fieldName);
    if (field?.hasError('required')) return 'Este campo es requerido';
    if (field?.hasError('email')) return 'Email no válido';
    if (field?.hasError('minlength')) return 'Mínimo 6 caracteres';
    if (field?.hasError('pattern')) return 'Solo números permitidos';
    if (field?.hasError('passwordMismatch')) return 'Las contraseñas no coinciden';
    return '';
  }

  // Submit con Swal loading
  register(): void {
    if (this.loading) return;

    // Forzamos touched para mostrar errores antes del submit
    this.credentialsForm.markAllAsTouched();
    this.basicForm.markAllAsTouched();
    this.contactForm.markAllAsTouched();

    if (this.credentialsForm.invalid || this.basicForm.invalid || this.contactForm.invalid) {
      return;
    }

    const objeto: RegisterUserModel = {
      firstName: (this.basicForm.value.firstName ?? '').trim(),
      lastName: (this.basicForm.value.lastName ?? '').trim(),
      identification: this.basicForm.value.identification,
      phoneNumber: this.contactForm.value.phoneNumber,
      address: (this.contactForm.value.address ?? '').trim(),
      cityId: this.contactForm.value.cityId,
      email: (this.credentialsForm.value.email ?? '').trim(),
      password: this.credentialsForm.value.password,
    };

    this.loading = true;

    Swal.fire({
      title: 'Creando usuario...',
      text: 'Por favor espera',
      allowOutsideClick: false,
      didOpen: () => Swal.showLoading(),
    });

    this._servicio
      .Register(objeto)
      .pipe(
        take(1),
        catchError((err) => {
          const msg =
            err?.error?.message ||
            err?.message ||
            'No se pudo completar el registro.';
          Swal.fire({ icon: 'error', title: 'Error', text: msg });
          return of({ isSuccess: false });
        }),
        finalize(() => (this.loading = false))
      )
      .subscribe((data: any) => {
        if (data?.isSuccess) {
          Swal.fire({
            icon: 'success',
            title: 'Usuario creado',
            text: 'El registro se completó correctamente.',
          }).then(() => this._router.navigate(['/auth/login']));
        } else {
          if (!Swal.isVisible() || Swal.isLoading()) {
            Swal.fire({
              icon: 'error',
              title: 'Oops...',
              text: 'Error al crear el usuario.',
            });
          }
        }
      });
  }
}
