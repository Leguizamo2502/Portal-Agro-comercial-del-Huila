import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RolFormPermissionSelectModel } from '../../../models/rolFormPermission/rolFormPermission.model';
import { RolFormPermissionService } from '../../../services/rolFormPermission/rol-form-permission.service';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { TableComponent } from "../../../../../shared/components/table/table.component";

@Component({
  selector: 'app-rol-form-permission-list',
  imports: [ButtonComponent, TableComponent],
  templateUrl: './rol-form-permission-list.component.html',
  styleUrl: './rol-form-permission-list.component.css'
})
export class RolFormPermissionListComponent implements OnInit{
  rolFormPermissionService = inject(RolFormPermissionService);
  rolFormPermissions: RolFormPermissionSelectModel[] = [];
  router = inject(Router);


  ngOnInit(): void {
    this.loadRolFormPermissions();
  }

  columns = [
    { key: 'rolName', label: 'Nombre de Rol' },
    { key: 'formName', label: 'Nombre del Formulario' },
    { key: 'permissionName', label: 'Nombre de Permiso' },

  ];

  onEdit(item: any) {
     const id = item.id;
    this.router.navigate(['/account/security/rolFormPermission/update', id]);
  }

  onDelete(item: any) {
    this.rolFormPermissionService.deleteLogic(item.id).subscribe(()=>{
      this.loadRolFormPermissions();
      // console.log("borrado")
    })
  }

  loadRolFormPermissions() {
    this.rolFormPermissionService.getAll().subscribe((data) => {
      this.rolFormPermissions = data;
    });
  }
}
