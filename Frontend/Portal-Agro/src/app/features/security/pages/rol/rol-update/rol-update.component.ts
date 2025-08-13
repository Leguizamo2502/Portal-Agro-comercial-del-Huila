import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { RolSelectModel, RolRegisterModel } from '../../../models/rol/rol.model';
import { RolService } from '../../../services/rol/rol.service';
import { PermissionFormComponent } from "../../permission/permission-form/permission-form.component";

@Component({
  selector: 'app-rol-update',
  imports: [PermissionFormComponent],
  templateUrl: './rol-update.component.html',
  styleUrl: './rol-update.component.css'
})
export class RolUpdateComponent {
    rolService = inject(RolService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: RolSelectModel;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.rolService.getById(this.id).subscribe((rol) => {
      this.model = rol;
    });
  }

  save(rol: RolRegisterModel) {
    this.rolService.update(this.id, rol).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Rol Actualizado',
          text: 'El rol se ha actualiazo correctamente',
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
          text: 'No se pudo actualizar el rol.',
        });

        console.error(error);
      },
    });
  }
}
