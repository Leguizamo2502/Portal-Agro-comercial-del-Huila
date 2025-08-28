// city-update.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { CityFormComponent } from '../city-form/city-form.component';
import { CityService } from '../../../services/city/city.service';
import { CitySelectModel, CityRegisterModel } from '../../../models/city/city.model';
import { DepartmentSelectModel } from '../../../models/department/department.model';
import { DepartmentService } from '../../../services/department/department.service';

// el form emite SIN id
type CityPayload = Omit<CityRegisterModel, 'id'>; // { name: string; DepartmentId: number }

@Component({
  selector: 'app-city-update',
  standalone: true,
  imports: [CityFormComponent],
  templateUrl: './city-update.component.html',
  styleUrl: './city-update.component.css'
})
export class CityUpdateComponent implements OnInit {
  cityService = inject(CityService);
  departmentService = inject(DepartmentService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: CitySelectModel;
  departments: DepartmentSelectModel[] = [];

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.departmentService.getAll().subscribe(d => (this.departments = d));
    this.cityService.getById(this.id).subscribe(c => (this.model = c));
  }

  // <-- aceptar payload SIN id y convertirlo al DTO que espera el servicio (CON id)
  save(payload: CityPayload) {
    const body: CityRegisterModel = {
      id: this.id,
      name: (payload.name ?? '').trim(),
      // Aseguramos número por si viene string desde el form
      DepartmentId: Number(payload.DepartmentId),
    };

    this.cityService.update(this.id, body).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Ciudad Actualizada',
          text: 'La ciudad se ha actualizado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => this.router.navigate(['/account/parameters/city']));
      },
      error: () => {
        Swal.fire({ icon: 'error', title: 'Error', text: 'No se pudo actualizar la ciudad.' });
      },
    });
  }
}
