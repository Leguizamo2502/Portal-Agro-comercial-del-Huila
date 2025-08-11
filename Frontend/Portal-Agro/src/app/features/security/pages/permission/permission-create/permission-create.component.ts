import { Component, inject } from '@angular/core';
import { PermissionService } from '../../../services/permission/permission.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { PermissionRegisterModel } from '../../../models/permission/permission.model';
import { PermissionFormComponent } from "../permission-form/permission-form.component";

@Component({
  selector: 'app-permission-create',
  imports: [PermissionFormComponent],
  templateUrl: './permission-create.component.html',
  styleUrl: './permission-create.component.css'
})
export class PermissionCreateComponent {
  permissionService = inject(PermissionService);
  router = inject(Router);


  saveChange(permission: PermissionRegisterModel) {
      this.permissionService.create(permission).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Permiso creado',
            text: 'El permiso se ha guardado correctamente',
            confirmButtonText: 'Aceptar',
          }).then(() => {
            this.router.navigate(['/account/security/permission']);
          });
  
          console.log(permission);
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
