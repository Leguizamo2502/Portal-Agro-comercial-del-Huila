import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { PersonUpdateModel, UserSelectModel } from '../../../../Core/Models/user.model';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { DepartmentModel, CityModel } from '../../../../shared/models/location/location.model';
import { LocationService } from '../../../../shared/services/location/location.service';

@Component({
  selector: 'app-update-person',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    ButtonComponent
  ],
  templateUrl: './update-person.component.html',
  styleUrl: './update-person.component.css'
})
export class UpdatePersonComponent implements OnInit {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private locationSvc = inject(LocationService);

  title = 'Actualizar datos personales';
  person?: UserSelectModel;
  isLoading = false;

  // Catálogos
  departments: DepartmentModel[] = [];
  cities: CityModel[] = [];

  // Flags de carga
  isLoadingDepartments = false;
  isLoadingCities = false;

  // Flag para evitar efectos colaterales durante la precarga
  private initializing = false;

  form = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(2)]],
    lastName: ['', [Validators.required, Validators.minLength(2)]],
    address: ['', [Validators.required, Validators.minLength(4)]],
    phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{7,15}$/)]],
    departmentId: [null as number | null, [Validators.required]],
    cityId: [{ value: null as number | null, disabled: true }, [Validators.required]],
  });

  get f() { return this.form.controls; }

  ngOnInit(): void {
    this.loadDepartments();
    this.loadPerson();
    this.handleDepartmentChanges();
  }

  private loadDepartments(): void {
    this.isLoadingDepartments = true;
    this.locationSvc.getDepartment().subscribe({
      next: (deps) => {
        this.departments = deps ?? [];
      },
      error: (err) => {
        Swal.fire({ icon: 'error', title: 'Error', text: err?.error?.message ?? 'No se pudieron cargar los departamentos.' });
      },
      complete: () => this.isLoadingDepartments = false
    });
  }

  private handleDepartmentChanges(): void {
    this.form.get('departmentId')!.valueChanges.subscribe((deptId) => {
      if (!deptId) {
        this.cities = [];
        this.form.get('cityId')!.reset();
        this.form.get('cityId')!.disable({ emitEvent: false });
        return;
      }
      // Si estamos inicializando, no reseteamos nada aquí; solo cargamos y preseleccionamos.
      const preselect = this.initializing ? this.person?.cityId : undefined;
      this.fetchCitiesForDept(deptId as number, preselect);
    });
  }

  private fetchCitiesForDept(deptId: number, preselectCityId?: number): void {
    this.isLoadingCities = true;
    // Si NO estamos en precarga, limpiamos para UX consistente
    if (!this.initializing) {
      this.cities = [];
      this.form.get('cityId')!.reset(undefined, { emitEvent: false });
      this.form.get('cityId')!.disable({ emitEvent: false });
    }

    this.locationSvc.getCity(deptId).subscribe({
      next: (cities) => {
        this.cities = cities ?? [];
        this.form.get('cityId')!.enable({ emitEvent: false });

        if (preselectCityId) {
          const exists = this.cities.some(c => c.id === preselectCityId);
          if (exists) {
            this.form.get('cityId')!.setValue(preselectCityId, { emitEvent: false });
          }
        }
      },
      error: (err) => {
        Swal.fire({ icon: 'error', title: 'Error', text: err?.error?.message ?? 'No se pudieron cargar las ciudades.' });
      },
      complete: () => this.isLoadingCities = false
    });
  }

  private loadPerson(): void {
    this.isLoading = true;
    this.initializing = true;

    this.auth.GetDataBasic().subscribe({
      next: (data) => {
        this.person = data;

        // Precarga de campos básicos
        this.form.patchValue({
          firstName: data.firstName,
          lastName: data.lastName,
          address: data.address,
          phoneNumber: data.phoneNumber
        }, { emitEvent: false });

        // Precarga de Department y City: seteamos departmentId sin emitir evento,
        // luego cargamos ciudades y preseleccionamos cityId manualmente.
        if (data.departmentId) {
          this.form.get('departmentId')!.setValue(data.departmentId, { emitEvent: false });
          this.fetchCitiesForDept(data.departmentId, data.cityId ?? undefined);
        }

        this.form.markAsPristine();
      },
      error: (err) => {
        Swal.fire({ icon: 'error', title: 'Error', text: err?.error?.message ?? 'No se pudieron cargar los datos.' });
      },
      complete: () => {
        this.isLoading = false;
        this.initializing = false;
      }
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    if (this.form.pristine) {
      Swal.fire({ icon: 'info', title: 'Sin cambios', text: 'No realizaste modificaciones.' });
      return;
    }

    const dto: PersonUpdateModel = {
      firstName: this.form.value.firstName!,
      lastName: this.form.value.lastName!,
      address: this.form.value.address!,
      phoneNumber: this.form.value.phoneNumber!,
      cityId: this.form.value.cityId as number,
    };

    this.isLoading = true;
    this.auth.UpdatePerson(dto).subscribe({
      next: () => {
        Swal.fire({ icon: 'success', title: 'Datos actualizados' });
        this.form.markAsPristine();
        this.router.navigate(['/account/info']);
      },
      error: (err) => {
        Swal.fire({ icon: 'error', title: 'Error', text: err?.error?.message ?? 'No se pudo actualizar la información.' });
      },
      complete: () => this.isLoading = false
    });
  }

  cancel(): void {
    if (this.person) {
      this.initializing = true;

      this.form.patchValue({
        firstName: this.person.firstName,
        lastName: this.person.lastName,
        address: this.person.address,
        phoneNumber: this.person.phoneNumber,
        departmentId: this.person.departmentId ?? null
      }, { emitEvent: false });

      if (this.person.departmentId) {
        this.fetchCitiesForDept(this.person.departmentId, this.person.cityId ?? undefined);
      } else {
        this.cities = [];
        this.form.get('cityId')!.reset();
        this.form.get('cityId')!.disable({ emitEvent: false });
      }

      this.form.markAsPristine();
      this.initializing = false;
    }
    this.router.navigate(['/account/info']);
  }
}
