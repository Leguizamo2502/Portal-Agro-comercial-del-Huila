import { ModuleService } from './../../../services/module/module.service';
import { ModuleModuleComponent } from './../module-module/module-module.component';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ModuleRegisterModel } from '../../../models/module/module.model';

@Component({
  selector: 'app-form-create',
  imports: [ModuleModuleComponent],
  templateUrl: '../module-create/module-create.component.html',
  styleUrl: '../module-create/module-create.component.css',
})
export class ModuleCreateComponent {
  moduleService = inject(ModuleService);
  router = inject(Router);

  saveChange(module: ModuleRegisterModel) {
    this.moduleService.create(module).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Modulo creado',
          text: 'El Modulo se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/module']);
        });

        console.log(module);
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo guardar el modulo.',
        });

        console.error(error);
      },
    });
  }
}
