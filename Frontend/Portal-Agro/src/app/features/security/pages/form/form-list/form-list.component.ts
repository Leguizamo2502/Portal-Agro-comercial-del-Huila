import { Component, inject, OnInit } from '@angular/core';
import { FormService } from '../../../services/form/form.service';
import { formSelectModel } from '../../../models/form/form.model';
import { TableComponent } from "../../../../../shared/components/table/table.component";
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-form-list',
  imports: [TableComponent,CommonModule,RouterLink],
  templateUrl: './form-list.component.html',
  styleUrl: './form-list.component.css'
})
export class FormListComponent implements OnInit{
  formService = inject(FormService);
  forms:formSelectModel[]=[]
  
  ngOnInit(): void {
    this.loadForm();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'description', label: 'Description' },
    { key: 'url', label: 'Url' },
  ];

  onEdit(item: any) {
    console.log('Editar', item);
  }

  onDelete(item: any) {
    this.formService.deleteLogic(item.id).subscribe(()=>{
      this.loadForm();
    })
  }

  
  loadForm(){
    this.formService.getAll().subscribe((data)=>{
      this.forms = data;
      console.log(data);
    })
  }
}
