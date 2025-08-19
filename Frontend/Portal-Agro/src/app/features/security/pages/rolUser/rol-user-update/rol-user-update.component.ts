import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { RolService } from '../../../services/rol/rol.service';
import { RolUserService } from '../../../services/rolUser/rol-user.service';
import { RolUserSelectModel, RolUserRegisterModel, RolUserFormComponent } from '../rol-user-form/rol-user-form.component';
import { UserService } from '../../../services/user/user.service';
import { UserSelectModel } from '../../../../../Core/Models/user.model';

@Component({
  selector: 'app-rol-user-update',
  imports: [RolUserFormComponent],
  templateUrl: './rol-user-update.component.html',
  styleUrl: './rol-user-update.component.css'
})
export class RolUserUpdateComponent implements OnInit{
  rolUserService = inject(RolUserService);
  rolService = inject(RolService);
  userService = inject(UserService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: RolUserSelectModel;
  rols: RolSelectModel[] = [];
  users: UserSelectModel[] = [];


  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

   
    this.rolService.getAll().subscribe(data => {
      this.rols = data;
    });

    this.userService.getUser().subscribe(data => {
      this.users = data;
    });

  
    this.rolUserService.getById(this.id).subscribe(rolUser => {
      this.model = rolUser;
    });
  }

  save(rolUser: RolUserRegisterModel) {
    this.rolUserService.update(this.id, rolUser).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Rol y Usuario Actualizado',
          text: 'El Rol y Usuario se ha actualizado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/rolUser']);
        });
      },
      error: () => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el rol y usuario.',
        });
      },
    });
  }
}
