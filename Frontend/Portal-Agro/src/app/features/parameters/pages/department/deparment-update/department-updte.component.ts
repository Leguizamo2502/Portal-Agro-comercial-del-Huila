import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { DepartmentFormComponent } from '../deparment-form/deparment-form.component';
import { DepartmentRegisterModel, DepartmentSelectModel } from '../../../models/department/department.model';
import { DepartmentService } from '../../../services/department/department.service';


@Component({
  selector: 'app-permission-update',
  imports: [DepartmentFormComponent],
  templateUrl: './department-updte.component.html',
  styleUrl: './department-updte.component.css'
})
export class DepartmentUpdateComponent implements OnInit {
  departmentService = inject(DepartmentService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: DepartmentSelectModel;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.departmentService.getById(this.id).subscribe((department) => {
      this.model = department;
    });
  }

  save(department: DepartmentRegisterModel) {
    this.departmentService.update(this.id, department).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Permiso Actualizado',
          text: 'El permiso se ha actualiazo correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/parameters/department']);
        });

        console.log(department);
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el Departamento.',
        });

        console.error(error);
      },
    });
  }
}
