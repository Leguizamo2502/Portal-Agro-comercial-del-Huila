import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CityFormComponent } from '../city-form/city-form.component';
import { CityRegisterModel } from '../../../models/city/city.model';
import { CityService } from '../../../services/city/city.service';
import { DepartmentService } from '../../../services/department/department.service';
import { DepartmentSelectModel } from '../../../models/department/department.model';

@Component({
  selector: 'app-city-create',
  imports: [CityFormComponent],
  templateUrl: './city-create.component.html',
  styleUrl: './city-create.component.css'
})
export class CityCreateComponent implements OnInit {
  cityService = inject(CityService);
  departmentService = inject(DepartmentService);
  router = inject(Router);

  departments: DepartmentSelectModel[] = [];

  ngOnInit(): void {
    // Cargar departamentos
    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (err) => {
        console.error('Error cargando departamentos', err);
      }
    });
  }

  saveChange(city: CityRegisterModel) {
    this.cityService.create(city).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Ciudad creada',
          text: 'La ciudad se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/parameters/city']);
        });
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo guardar la ciudad.',
        });
        console.error(error);
      },
    });
  }
}
