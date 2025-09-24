import { Component, inject, OnInit } from '@angular/core';
import { UserService } from '../../../services/user/user.service';
import { UserSelectModel } from '../../../../../Core/Models/user.model';
import { Router, ActivatedRoute } from '@angular/router';
import { TableComponent } from "../../../../../shared/components/table/table.component";

@Component({
  selector: 'app-user-list',
  imports: [TableComponent],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
})
export class UserListComponent implements OnInit {
  private userService = inject(UserService);
  users: UserSelectModel[] = [];
  router = inject(Router);
  route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.loadUsers();
  }

  columns = [
  { key: 'firstName', label: 'Nombre completo' },
  { key: 'identification', label: 'Identificación' },
  { key: 'email', label: 'Correo electrónico' },
  { key: 'phoneNumber', label: 'Teléfono' },
  { key: 'address', label: 'Dirección' },
  { key: 'cityName', label: 'Ciudad' },
  { key: 'active', label: 'Estado' },

  { key: 'roles', label: 'Roles' }
];


  onEdit(item: any) {
    const id = item.id;
    this.router.navigate(['/account/security/user/update', id]);
  }

  onDelete(item: any) {
    this.userService.deleteLogic(item.id).subscribe(() => {
      this.loadUsers();
    });
  }

  loadUsers() {
  this.userService.getUser().subscribe((data) => {
      console.log(data)

    this.users = data.map(u => ({
      ...u,
      active: u.active ? 'Activo' : 'Desactivo'
    })) as any; // si quieres que active sea string
  });
}
}
