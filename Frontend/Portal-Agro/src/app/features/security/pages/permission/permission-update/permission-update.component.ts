import { Component, inject, OnInit } from '@angular/core';
import { FormService } from '../../../services/form/form.service';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { PermissionRegisterModel, PermissionSelectModel } from '../../../models/permission/permission.model';
import { PermissionService } from '../../../services/permission/permission.service';
import { PermissionFormComponent } from "../permission-form/permission-form.component";

@Component({
  selector: 'app-permission-update',
  imports: [PermissionFormComponent],
  templateUrl: './permission-update.component.html',
  styleUrl: './permission-update.component.css'
})
export class PermissionUpdateComponent implements OnInit {
  permissionService = inject(PermissionService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: PermissionSelectModel;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.permissionService.getById(this.id).subscribe((permission) => {
      this.model = permission;
    });
  }

  save(permission: PermissionRegisterModel) {
    this.permissionService.update(this.id, permission).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Permiso Actualizado',
          text: 'El permiso se ha actualiazo correctamente',
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
          text: 'No se pudo actualizar el permiso.',
        });

        console.error(error);
      },
    });
  }
}
