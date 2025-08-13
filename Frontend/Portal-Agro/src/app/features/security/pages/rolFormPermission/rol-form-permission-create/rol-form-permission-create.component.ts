import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { FormSelectModel } from '../../../models/form/form.model';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { FormService } from '../../../services/form/form.service';
import { RolService } from '../../../services/rol/rol.service';
import { RolFormPermissionService } from '../../../services/rolFormPermission/rol-form-permission.service';
import { RolFormPermissionRegisterModel } from '../../../models/rolFormPermission/rolFormPermission.model';
import { PermissionService } from '../../../services/permission/permission.service';
import { PermissionSelectModel } from '../../../models/permission/permission.model';
import { RolFormPermissionFormComponent } from "../rol-form-permission-form/rol-form-permission-form.component";

@Component({
  selector: 'app-rol-form-permission-create',
  imports: [RolFormPermissionFormComponent],
  templateUrl: './rol-form-permission-create.component.html',
  styleUrl: './rol-form-permission-create.component.css',
})
export class RolFormPermissionCreateComponent {
  rolFormPermissionService = inject(RolFormPermissionService);
  formService = inject(FormService);
  rolService = inject(RolService);
  permissionService = inject(PermissionService);

  router = inject(Router);

  forms: FormSelectModel[] = [];
  rols: RolSelectModel[] = [];
  permissions: PermissionSelectModel[] = [];

  ngOnInit(): void {
    this.formService.getAll().subscribe({
      next: (data) => {
        this.forms = data;
      },
      error: (err) => {
        console.error('Error cargando formularios', err);
      },
    });

    this.rolService.getAll().subscribe({
      next: (data) => {
        this.rols = data;
      },
      error: (err) => {
        console.error('Error cargando roles', err);
      },
    });

    this.permissionService.getAll().subscribe({
      next: (data) => {
        this.permissions = data;
      },
      error: (err) => {
        console.error('Error cargando permisos', err);
      },
    });
  }

  saveChange(rolFormPermission: RolFormPermissionRegisterModel) {
    this.rolFormPermissionService.create(rolFormPermission).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Rol, Formulario y permiso creado',
          text: 'El Rol, Formulario y permiso se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/rolFormPermission']);
        });
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo guardar el Rol, Formulario y permiso.',
        });
        console.error(error);
      },
    });
  }
}
