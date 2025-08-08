import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { DepartmentRegisterModel } from '../../../models/department/department.model';
import { DepartmentService } from '../../../services/department/department.service';
import { DepartmentFormComponent } from '../deparment-form/deparment-form.component';


@Component({
  selector: 'app-permission-create',
  imports: [DepartmentFormComponent],
  templateUrl: './deparment-create.component.html',
  styleUrl: './deparment-create.component.css'
})
export class DepartmentCreateComponent {
  departmentService = inject(DepartmentService);
  router = inject(Router);


  saveChange(department: DepartmentRegisterModel) {
      this.departmentService.create(department).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Departamento creado',
            text: 'El Departamento se ha guardado correctamente',
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
            text: 'No se pudo guardar el permiso.',
          });
  
          console.error(error);
        },
      });
    }
}
