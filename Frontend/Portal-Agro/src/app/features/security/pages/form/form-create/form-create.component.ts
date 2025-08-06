import { Component, inject } from '@angular/core';
import { FormService } from '../../../services/form/form.service';
import { Router } from '@angular/router';
import { formRegisterModel } from '../../../models/form/form.model';
import { FormFormComponent } from '../form-form/form-form.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-form-create',
  imports: [FormFormComponent],
  templateUrl: './form-create.component.html',
  styleUrl: './form-create.component.css',
})
export class FormCreateComponent {
  formService = inject(FormService);
  router = inject(Router);

  saveChange(form: formRegisterModel) {
    this.formService.create(form).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Formulario creado',
          text: 'El formulario se ha guardado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/security/form']);
        });

        console.log(form);
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo guardar el formulario.',
        });

        console.error(error);
      },
    });
  }
}
