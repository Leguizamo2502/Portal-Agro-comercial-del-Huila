import { Component, inject, OnInit } from '@angular/core';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { UserSelectModel } from '../../../../../Core/Models/user.model';
import { RolService } from '../../../services/rol/rol.service';
import { RolUserService } from '../../../services/rolUser/rol-user.service';
import { UserService } from '../../../services/user/user.service';
import { Router } from '@angular/router';
import { RolUserRegisterModel, RolUserFormComponent } from '../rol-user-form/rol-user-form.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-rol-user-create',
  imports: [RolUserFormComponent],
  templateUrl: './rol-user-create.component.html',
  styleUrl: './rol-user-create.component.css'
})
export class RolUserCreateComponent implements OnInit{
  rolUserService = inject(RolUserService);
  rolService = inject(RolService);
  userService = inject(UserService);

  router = inject(Router);

  rols: RolSelectModel[] = [];
  users: UserSelectModel[] = [];


  ngOnInit(): void {
    this.rolService.getAll().subscribe({
      next: (data) => {
        this.rols = data;
      },
      error: (err) => {
        console.error('Error cargando roles', err);
      }
    });

    this.userService.getUser().subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        console.error('Error cargando usuarios', err);
      }
    });


  }

  saveChange(rolUser: RolUserRegisterModel) {
    this.rolUserService.create(rolUser).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Rol y usuario creado',
          text: 'El rol y usuario se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/rolUser']);
        });
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo guardar el rol y usuario.',
        });
        console.error(error);
      },
    });
  }
}
