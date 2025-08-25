import { Component, inject, OnInit } from '@angular/core';
import { FormService } from '../../../services/form/form.service';
import { TableComponent } from '../../../../../shared/components/table/table.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { FormSelectModel } from '../../../models/form/form.model';

@Component({
  selector: 'app-form-list',
  imports: [TableComponent, CommonModule, ButtonComponent],
  templateUrl: './form-list.component.html',
  styleUrl: './form-list.component.css',
})
export class FormListComponent implements OnInit {
  formService = inject(FormService);
  forms: FormSelectModel[] = [];
  router = inject(Router);
  // route = inject(ActivatedRoute);

  ngOnInit(): void {
    this.loadForm();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'description', label: 'Description' },
    { key: 'url', label: 'Url' },
  ];

  onEdit(item: any) {
    const id = item.id;
    this.router.navigate(['/account/security/form/update', id]);
  }

  onDelete(item: any) {
    this.formService.deleteLogic(item.id).subscribe(() => {
      this.loadForm();
    });
  }

  loadForm() {
    this.formService.getAll().subscribe((data) => {
      this.forms = data;
      // console.log(data);
    });
  }
}
