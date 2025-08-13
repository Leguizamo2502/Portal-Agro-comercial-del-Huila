import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { FormModuleRegisterModel } from '../../../models/formModule/formModule.model';
import { ModuleSelectModel } from '../../../models/module/module.model';
import { FormService } from '../../../services/form/form.service';
import { FormModuleService } from '../../../services/formModule/form-module.service';
import { ModuleService } from '../../../services/module/module.service';
import { FormSelectModel } from '../../../models/form/form.model';
import { CommonModule } from '@angular/common';
import { FormModuleFormComponent } from "../form-module-form/form-module-form.component";

@Component({
  selector: 'app-form-module-create',
  imports: [CommonModule, FormModuleFormComponent],
  templateUrl: './form-module-create.component.html',
  styleUrl: './form-module-create.component.css'
})
export class FormModuleCreateComponent implements OnInit{
  formModuleService = inject(FormModuleService);
  formService = inject(FormService);
  moduleService = inject(ModuleService);

  router = inject(Router);

  forms: FormSelectModel[] = [];
  modules: ModuleSelectModel[] = [];


  ngOnInit(): void {
    this.formService.getAll().subscribe({
      next: (data) => {
        this.forms = data;
      },
      error: (err) => {
        console.error('Error cargando formularios', err);
      }
    });

    this.moduleService.getAll().subscribe({
      next: (data) => {
        this.modules = data;
      },
      error: (err) => {
        console.error('Error cargando modulos', err);
      }
    });


  }

  saveChange(formModule: FormModuleRegisterModel) {
    this.formModuleService.create(formModule).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Modulo y Formulario creado',
          text: 'El Modulo y Formulario se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/formModule']);
        });
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo guardar el Modulo y Formulario.',
        });
        console.error(error);
      },
    });
  }
}
