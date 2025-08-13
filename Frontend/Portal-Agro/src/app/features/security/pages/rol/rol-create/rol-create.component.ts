import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { RolRegisterModel } from '../../../models/rol/rol.model';
import { RolService } from '../../../services/rol/rol.service';
import { PermissionFormComponent } from "../../permission/permission-form/permission-form.component";

@Component({
  selector: 'app-rol-create',
  imports: [PermissionFormComponent],
  templateUrl: './rol-create.component.html',
  styleUrl: './rol-create.component.css'
})
export class RolCreateComponent {
  rolService = inject(RolService);
  router = inject(Router);


  saveChange(rol: RolRegisterModel) {
      this.rolService.create(rol).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Rol creado',
            text: 'El rol se ha guardado correctamente',
            confirmButtonText: 'Aceptar',
          }).then(() => {
            this.router.navigate(['/account/security/rol']);
          });
  
          console.log(rol);
        },
        error: (error) => {
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo guardar el rol.',
          });
  
          console.error(error);
        },
      });
    }
}
