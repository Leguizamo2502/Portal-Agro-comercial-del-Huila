import { Component, inject, OnInit } from '@angular/core';
import { PermissionService } from '../../../services/permission/permission.service';
import { PermissionSelectModel } from '../../../models/permission/permission.model';
import { TableComponent } from "../../../../../shared/components/table/table.component";
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-permission-list',
  imports: [TableComponent,RouterLink,CommonModule,MatButton],
  templateUrl: './permission-list.component.html',
  styleUrl: './permission-list.component.css',
})
export class PermissionListComponent implements OnInit {
  permissionService = inject(PermissionService);
  permissions: PermissionSelectModel[] = [];
  router = inject(Router);


  ngOnInit(): void {
    this.loadPermissions();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'description', label: 'Description' },
  ];

  onEdit(item: any) {
     const id = item.id;
    this.router.navigate(['/account/security/permission/update', id]);
  }

  onDelete(item: any) {
    this.permissionService.deleteLogic(item.id).subscribe(()=>{
      this.loadPermissions();
      // console.log("borrado")
    })
  }

  loadPermissions() {
    this.permissionService.getAll().subscribe((data) => {
      this.permissions = data;
    });
  }
}
