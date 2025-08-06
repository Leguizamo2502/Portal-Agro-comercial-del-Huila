import {
  Component,
  inject,
  Input,
  numberAttribute,
  OnInit,
} from '@angular/core';
import { ModuleService } from '../../../services/module/module.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ModuleRegisterModel,
  ModuleSelectModel,
} from '../../../models/module/module.model';
import { ModuleModuleComponent } from '../module-module/module-module.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-module-update',
  imports: [ModuleModuleComponent],
  templateUrl: './module-update.component.html',
  styleUrl: './module-update.component.css',
})
export class ModuleUpdateComponent implements OnInit {
  moduleService = inject(ModuleService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: ModuleSelectModel;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.moduleService.getById(this.id).subscribe((module) => {
      this.model = module;
    });
  }

  save(module: ModuleRegisterModel) {
    this.moduleService.update(this.id, module).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Formulario Actualizado',
          text: 'El modulo se ha actualiazo correctamente',
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
          text: 'No se pudo actualizar el modulo.',
        });

        console.error(error);
      },
    });
  }
}
