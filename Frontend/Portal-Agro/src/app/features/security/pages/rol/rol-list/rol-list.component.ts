import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RolSelectModel } from '../../../models/rol/rol.model';
import { RolService } from '../../../services/rol/rol.service';
import { ButtonComponent } from "../../../../../shared/components/button/button.component";
import { TableComponent } from "../../../../../shared/components/table/table.component";

@Component({
  selector: 'app-rol-list',
  imports: [ButtonComponent, TableComponent],
  templateUrl: './rol-list.component.html',
  styleUrl: './rol-list.component.css'
})
export class RolListComponent implements OnInit{
  rolService = inject(RolService);
  rols: RolSelectModel[] = [];
  router = inject(Router);


  ngOnInit(): void {
    this.loadRols();
  }

  columns = [
    { key: 'name', label: 'Nombre' },
    { key: 'description', label: 'Description' },
  ];

  onEdit(item: any) {
     const id = item.id;
    this.router.navigate(['/account/security/rol/update', id]);
  }

  onDelete(item: any) {
    this.rolService.deleteLogic(item.id).subscribe(()=>{
      this.loadRols();
      // console.log("borrado")
    })
  }

  loadRols() {
    this.rolService.getAll().subscribe((data) => {
      this.rols = data;
    });
  }
}
