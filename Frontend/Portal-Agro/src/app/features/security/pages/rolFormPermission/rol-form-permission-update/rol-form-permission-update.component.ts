import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { FormSelectModel } from '../../../models/form/form.model';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { RolFormPermissionSelectModel, RolFormPermissionRegisterModel } from '../../../models/rolFormPermission/rolFormPermission.model';
import { FormService } from '../../../services/form/form.service';
import { RolService } from '../../../services/rol/rol.service';
import { RolFormPermissionService } from '../../../services/rolFormPermission/rol-form-permission.service';
import { PermissionSelectModel } from '../../../models/permission/permission.model';
import { PermissionService } from '../../../services/permission/permission.service';
import { RolFormPermissionFormComponent } from "../rol-form-permission-form/rol-form-permission-form.component";

@Component({
  selector: 'app-rol-form-permission-update',
  imports: [RolFormPermissionFormComponent],
  templateUrl: './rol-form-permission-update.component.html',
  styleUrl: './rol-form-permission-update.component.css'
})
export class RolFormPermissionUpdateComponent implements OnInit{
  rolFormPermissionService = inject(RolFormPermissionService);
  formService = inject(FormService);
  rolService = inject(RolService);
  permissionService = inject(PermissionService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: RolFormPermissionSelectModel;
  forms: FormSelectModel[] = [];
  rols: RolSelectModel[] = [];
  permissions: PermissionSelectModel[]=[];


  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

   
    this.formService.getAll().subscribe(data => {
      this.forms = data;
    });

    this.rolService.getAll().subscribe(data => {
      this.rols = data;
    });
    
    this.permissionService.getAll().subscribe((data)=>{
      this.permissions = data;
    })

  
    this.rolFormPermissionService.getById(this.id).subscribe(rolFormPermission => {
      this.model = rolFormPermission;
    });
  }

  save(rolFormPermission: RolFormPermissionRegisterModel) {
    this.rolFormPermissionService.update(this.id, rolFormPermission).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Rol, Formulario y Permiso Actualizado',
          text: 'El Rol, Formulario y Permiso se ha actualizado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/rolFormPermission']);
        });
      },
      error: () => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el Rol, Formulario y Permiso.',
        });
      },
    });
  }
}
