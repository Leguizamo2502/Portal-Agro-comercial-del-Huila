import { Component, inject, OnInit } from '@angular/core';
import { TableComponent } from '../../../../../shared/components/table/table.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ModuleService } from '../../../services/module/module.service';
import { ModuleSelectModel } from '../../../models/module/module.model';

@Component({
  selector: 'app-form-list',
  imports: [TableComponent, CommonModule, RouterLink],
  templateUrl: './module-list.component.html',
  styleUrl: './module-list.component.css',
})
export class ModuleListComponent implements OnInit {
  moduleService = inject(ModuleService);
  modules: ModuleSelectModel[] = [];
  router = inject(Router);
  route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.loadForm();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'description', label: 'Description' },
  ];

  onEdit(item: any) {
    const id = item.id;
    this.router.navigate(['/account/security/module/update', id]);

  }

  onDelete(item: any) {
    this.moduleService.deleteLogic(item.id).subscribe(() => {
      this.loadForm();
    });
  }

  loadForm() {
    this.moduleService.getAll().subscribe((data) => {
      this.modules = data;
      // console.log(data);
    });
  }
}
