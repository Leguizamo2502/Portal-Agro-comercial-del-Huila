import { Component, inject, Inject, OnInit } from '@angular/core';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { PersonUpdateModel, UserSelectModel } from '../../../../Core/Models/user.model';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-update-person',
  imports: [CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    ButtonComponent],
  templateUrl: './update-person.component.html',
  styleUrl: './update-person.component.css'
})
export class UpdatePersonComponent implements OnInit{
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  title = 'Actualizar datos personales';
  person?: UserSelectModel;
  isLoading = false;

  form = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(2)]],
    lastName: ['', [Validators.required, Validators.minLength(2)]],
    address: ['', [Validators.required, Validators.minLength(4)]],
    phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{7,15}$/)]],
    // email: ['', [Validators.required, Validators.email]]
  });

  get f() { return this.form.controls; }

  ngOnInit(): void {
    this.loadPerson();
  }

  private loadPerson(): void {
    this.isLoading = true;
    this.auth.GetDataBasic().subscribe({
      next: (data) => {
        this.person = data;
        // Mapear UserSelectModel -> PersonUpdateModel
        this.form.patchValue({
          firstName: data.firstName,
          lastName: data.lastName,
          address: data.address,
          phoneNumber: data.phoneNumber,
          // email: data.email
        });
        this.form.markAsPristine();
      },
      error: (err) => {
        Swal.fire({ icon: 'error', title: 'Error', text: err?.error?.message ?? 'No se pudieron cargar los datos.' });
      },
      complete: () => this.isLoading = false
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    if (this.form.pristine) {
      Swal.fire({ icon: 'info', title: 'Sin cambios', text: 'No realizaste modificaciones.' });
      return;
    }

    const dto: PersonUpdateModel = this.form.getRawValue() as PersonUpdateModel;

    this.isLoading = true;
    this.auth.UpdatePerson(dto).subscribe({
      next: () => {
        Swal.fire({ icon: 'success', title: 'Datos actualizados' });
        this.form.markAsPristine();
        this.router.navigate(['/account/info']); // ajusta si tu ruta de destino es otra
      },
      error: (err) => {
        Swal.fire({ icon: 'error', title: 'Error', text: err?.error?.message ?? 'No se pudo actualizar la información.' });
      },
      complete: () => this.isLoading = false
    });
  }

  cancel(): void {
    // O vuelve a los datos originales o navega atrás
    if (this.person) {
      this.form.patchValue({
        firstName: this.person.firstName,
        lastName: this.person.lastName,
        address: this.person.address,
        phoneNumber: this.person.phoneNumber,
        // email: this.person.email
      });
      this.form.markAsPristine();
    }
    this.router.navigate(['/account/info']);
  }

 

}
