import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RolUserSelectModel } from '../../../models/rolUser/rolUser.model';
import { RolUserService } from '../../../services/rolUser/rol-user.service';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { TableComponent } from "../../../../../shared/components/table/table.component";

@Component({
  selector: 'app-rol-user-list',
  imports: [ButtonComponent, TableComponent],
  templateUrl: './rol-user-list.component.html',
  styleUrl: './rol-user-list.component.css'
})
export class RolUserListComponent implements OnInit{
  rolUserService = inject(RolUserService);
  rolUsers: RolUserSelectModel[] = [];
  router = inject(Router);


  ngOnInit(): void {
    this.loadRolUsers();
  }

  columns = [
    { key: 'userName', label: 'Nombre de Usuario' },
    { key: 'rolName', label: 'Nombre de Rol' },
  ];

  onEdit(item: any) {
     const id = item.id;
    this.router.navigate(['/account/security/rolUser/update', id]);
  }

  onDelete(item: any) {
    this.rolUserService.delete(item.id).subscribe(()=>{
      this.loadRolUsers();
      // console.log("borrado")
    })
  }

  loadRolUsers() {
    this.rolUserService.getAll().subscribe((data) => {
      this.rolUsers = data;
    });
  }
}
