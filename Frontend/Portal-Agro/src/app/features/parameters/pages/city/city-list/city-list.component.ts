import { Component, inject, OnInit } from '@angular/core';
import { TableComponent } from '../../../../../shared/components/table/table.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { CityService } from '../../../services/city/city.service';
import { CitySelectModel } from '../../../models/city/city.model';

@Component({
  selector: 'app-city-list',
  imports: [TableComponent, CommonModule, RouterLink,MatButton],
  templateUrl: './city-list.component.html',
  styleUrl: './city-list.component.css',
})
export class CityListComponent implements OnInit {
  cityService = inject(CityService);
  citys: CitySelectModel[] = [];
  router = inject(Router);
  // route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.loadForm();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'departmentName', label: 'Departamento' },
  ];

  onEdit(item: any) {
    const id = item.id;
    this.router.navigate(['/account/parameters/city/update', id]);

  }

  onDelete(item: any) {
    this.cityService.deleteLogic(item.id).subscribe(() => {
      this.loadForm();
    });
  }

  loadForm() {
    this.cityService.getAll().subscribe((data) => {
      this.citys = data;
      // console.log(data);
    });
  }
}
