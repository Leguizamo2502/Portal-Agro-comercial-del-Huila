import { Component, inject, OnInit } from '@angular/core';
import { FarmService } from '../../../../../shared/services/farm/farm.service';
import { Router } from '@angular/router';
import { FarmSelectModel } from '../../../../../shared/models/farm/farm.model';
import Swal from 'sweetalert2';
import { ButtonComponent } from '../../../../../shared/components/button/button.component';
import { CommonModule } from '@angular/common';
import { ContainerCardFlexComponent } from "../../../../../shared/components/cards/container-card-flex/container-card-flex.component";

@Component({
  selector: 'app-farm-list',
  imports: [ButtonComponent, CommonModule, ContainerCardFlexComponent],
  templateUrl: './farm-list.component.html',
  styleUrl: './farm-list.component.css',
})
export class FarmListComponent implements OnInit {
  private farmService = inject(FarmService);
  private router = inject(Router);

  farms: FarmSelectModel[] = [];

  ngOnInit(): void {
    this.loadFarms();
  }

  trackById = (_: number, f: FarmSelectModel) => f.id;

  loadFarms() {
    this.farmService.getByProducer().subscribe((data) => {
      this.farms = data;
    });
  }

  onEdit(p: FarmSelectModel) {
    this.router.navigate(['/account/producer/management/farm/update', p.id]);
  }

  onDelete(p: FarmSelectModel) {
    Swal.fire({
      title: '¿Eliminar finca?',
      text: `Se eliminará "${p.name}". Esta acción no se puede deshacer.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (!result.isConfirmed) return;

      this.farmService.delete(p.id).subscribe({
        next: () => {
          // Remueve localmente sin recargar toda la lista
          this.farms = this.farms.filter((x) => x.id !== p.id);
          Swal.fire('Eliminado', 'La finca fue eliminada.', 'success');
        },
        error: () => {
          Swal.fire('Error', 'No se pudo eliminar la finca.', 'error');
        },
      });
    });
  }
}
