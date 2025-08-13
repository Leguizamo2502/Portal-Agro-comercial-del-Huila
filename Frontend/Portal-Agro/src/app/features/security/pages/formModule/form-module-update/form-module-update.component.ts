import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { FormSelectModel } from '../../../models/form/form.model';
import { FormModuleSelectModel, FormModuleRegisterModel } from '../../../models/formModule/formModule.model';
import { ModuleSelectModel } from '../../../models/module/module.model';
import { FormService } from '../../../services/form/form.service';
import { FormModuleService } from '../../../services/formModule/form-module.service';
import { ModuleService } from '../../../services/module/module.service';
import { FormModuleFormComponent } from "../form-module-form/form-module-form.component";

@Component({
  selector: 'app-form-module-update',
  imports: [FormModuleFormComponent],
  templateUrl: './form-module-update.component.html',
  styleUrl: './form-module-update.component.css'
})
export class FormModuleUpdateComponent implements OnInit {
  formModuleService = inject(FormModuleService);
  formService = inject(FormService);
  moduleService = inject(ModuleService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: FormModuleSelectModel;
  forms: FormSelectModel[] = [];
  modules: ModuleSelectModel[] = [];


  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

   
    this.formService.getAll().subscribe(data => {
      this.forms = data;
    });

    this.moduleService.getAll().subscribe(data => {
      this.modules = data;
    });

  
    this.formModuleService.getById(this.id).subscribe(formModule => {
      this.model = formModule;
    });
  }

  save(formModule: FormModuleRegisterModel) {
    this.formModuleService.update(this.id, formModule).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Modulo y Formulario Actualizado',
          text: 'El Modulo y Formulario se ha actualizado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/formModule']);
        });
      },
      error: () => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar el Modulo y Formulario.',
        });
      },
    });
  }
}
