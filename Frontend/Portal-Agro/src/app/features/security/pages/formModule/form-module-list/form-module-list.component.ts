import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormModuleSelectModel } from '../../../models/formModule/formModule.model';
import { FormModuleService } from '../../../services/formModule/form-module.service';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { TableComponent } from "../../../../../shared/components/table/table.component";

@Component({
  selector: 'app-form-module-list',
  imports: [ButtonComponent, TableComponent],
  templateUrl: './form-module-list.component.html',
  styleUrl: './form-module-list.component.css'
})
export class FormModuleListComponent implements OnInit{
  formoduleService = inject(FormModuleService);
  formodules: FormModuleSelectModel[] = [];
  router = inject(Router);


  ngOnInit(): void {
    this.loadFormModules();
  }

  columns = [
    { key: 'formName', label: 'Nombre de Usuario' },
    { key: 'moduleName', label: 'Nombre de Rol' },
  ];

  onEdit(item: any) {
     const id = item.id;
    this.router.navigate(['/account/security/formModule/update', id]);
  }

  onDelete(item: any) {
    this.formoduleService.deleteLogic(item.id).subscribe(()=>{
      this.loadFormModules();
      // console.log("borrado")
    })
  }

  loadFormModules() {
    this.formoduleService.getAll().subscribe((data) => {
      this.formodules = data;
    });
  }
}
