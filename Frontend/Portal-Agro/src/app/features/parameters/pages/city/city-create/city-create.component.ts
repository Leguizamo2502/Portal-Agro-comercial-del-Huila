// city-create.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CityFormComponent } from '../city-form/city-form.component';
import { CityRegisterModel } from '../../../models/city/city.model';
import { CityService } from '../../../services/city/city.service';
import { DepartmentService } from '../../../services/department/department.service';
import { DepartmentSelectModel } from '../../../models/department/department.model';
import Swal from 'sweetalert2';

// lo que emite el form (sin id)
type CityPayload = { name: string; DepartmentId: number };

@Component({
  selector: 'app-city-create',
  standalone: true,
  imports: [CityFormComponent],
  templateUrl: './city-create.component.html',
  styleUrl: './city-create.component.css'
})
export class CityCreateComponent implements OnInit {
  private cityService = inject(CityService);
  private departmentService = inject(DepartmentService);
  private router = inject(Router);

  departments: DepartmentSelectModel[] = [];

  ngOnInit(): void {
    this.departmentService.getAll().subscribe({
      next: (data) => (this.departments = data),
      error: (err) => console.error('Error cargando departamentos', err),
    });
  }

  // <-- recibir payload SIN id y mapearlo al DTO del servicio
  saveChange(payload: CityPayload) {
    const body: CityRegisterModel = {
      id: 0, // o undefined si tu backend lo permite/ignora
      name: (payload.name ?? '').trim(),
      DepartmentId: Number(payload.DepartmentId),
    };

    this.cityService.create(body).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Ciudad creada',
          text: 'La ciudad se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => this.router.navigate(['/account/parameters/city']));
      },
      error: (error) => {
        Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo guardar la ciudad.' });
        console.error(error);
      },
    });
  }
}
