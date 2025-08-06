import { Component, inject, OnInit } from '@angular/core';
import { ModuleService } from '../../../services/module/module.service';
import { ModuleSelectModel } from '../../../models/module/module.model';
import { TableComponent } from "../../../../../shared/components/table/table.component";

@Component({
  selector: 'app-module-list',
  imports: [TableComponent],
  templateUrl: './module-list.component.html',
  styleUrl: './module-list.component.css'
})
export class ModuleListComponent implements OnInit{
  moduleService = inject(ModuleService);
  modules: ModuleSelectModel[]=[];
  
  ngOnInit(): void {
    this.loadModules();
    
  }


  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'description', label: 'Description' },
  ];

  onEdit(item: any) {
    console.log('Editar', item);
  }

  onDelete(item: any) {
    this.moduleService.deleteLogic(item.id).subscribe(()=>{
      this.loadModules();
    })
  }

  loadModules(){
    this.moduleService.getAll().subscribe((data)=>{
      this.modules = data;
      console.log(data);
    })
  }
  


}
