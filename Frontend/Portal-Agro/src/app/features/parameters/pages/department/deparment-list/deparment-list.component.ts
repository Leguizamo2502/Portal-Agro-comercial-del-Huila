import { DepartmentService } from './../../../services/department/department.service';
import { Component, inject, OnInit } from '@angular/core';
import { TableComponent } from '../../../../../shared/components/table/table.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DepartmentSelectModel } from '../../../models/department/department.model';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";

@Component({
  selector: 'app-deparment-list',
  imports: [TableComponent, CommonModule,ButtonComponent],
  templateUrl: './deparment-list.component.html',
  styleUrl: './deparment-list.component.css',
})
export class DeparmentListComponent implements OnInit {
  DepartmentService = inject(DepartmentService);
  departments: DepartmentSelectModel[] = [];
  router = inject(Router);
  // route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.loadForm();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
  ];

  onEdit(item: any) {
    const id = item.id;
    this.router.navigate(['/account/parameters/department/update', id]);

  }

  onDelete(item: any) {
    this.DepartmentService.deleteLogic(item.id).subscribe(() => {
      this.loadForm();
    });
  }

  loadForm() {
    this.DepartmentService.getAll().subscribe((data) => {
      this.departments = data;
       console.log(data);
    });
  }
}
