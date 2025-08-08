import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { CityFormComponent } from '../city-form/city-form.component';
import { CityService } from '../../../services/city/city.service';
import { CityRegisterModel, CitySelectModel } from '../../../models/city/city.model';


@Component({
  selector: 'app-city-update',
  imports: [CityFormComponent],
  templateUrl: './city-update.component.html',
  styleUrl: './city-update.component.css'
})
export class CityUpdateComponent implements OnInit {
  cityService = inject(CityService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  id!: number;
  model?: CitySelectModel;

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.id) return;

    this.cityService.getById(this.id).subscribe((city) => {
      this.model = city;
    });
  }

  save(city: CityRegisterModel) {
    this.cityService.update(this.id, city).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Ciudad Actualizado',
          text: 'La ciudad se ha actualizado correctamente',
          confirmButtonText: 'Aceptar',
        }).then(() => {
          this.router.navigate(['/account/parameters/city']);
        });

        console.log(city);
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'No se pudo actualizar la ciudad.',
        });

        console.error(error);
      },
    });
  }
}
