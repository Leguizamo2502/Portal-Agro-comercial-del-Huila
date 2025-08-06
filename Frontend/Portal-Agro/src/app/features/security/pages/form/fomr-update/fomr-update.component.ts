import {
  Component,
  inject,
  Input,
  numberAttribute,
  OnInit,
} from '@angular/core';
import { FormService } from '../../../services/form/form.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  formRegisterModel,
  formSelectModel,
} from '../../../models/form/form.model';
import { FormFormComponent } from '../form-form/form-form.component';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-fomr-update',
  imports: [FormFormComponent],
  templateUrl: './fomr-update.component.html',
  styleUrl: './fomr-update.component.css',
})
export class FomrUpdateComponent implements OnInit {
  formService = inject(FormService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: formSelectModel;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.formService.getById(this.id).subscribe((form) => {
      this.model = form;
    });
  }

  save(form: formRegisterModel) {
    this.formService.update(this.id, form).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Formulario Actualizado',
          text: 'El formulario se ha actualiazo correctamente',
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
          text: 'No se pudo actualizar el formulario.',
        });

        console.error(error);
      },
    });
  }
}
