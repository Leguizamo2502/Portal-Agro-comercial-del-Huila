// table.component.ts
import { Component, EventEmitter, Input, Output, ViewChild, OnChanges, SimpleChanges, inject } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, startWith } from 'rxjs/operators';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-table',
  imports: [
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent implements OnChanges {
  @Input() columns: { key: string; label: string }[] = [];
  @Input() data: any[] = [];
  /**
   * Si no la pasas, buscará en todas las columnas definidas en `columns`.
   * Puedes pasar, por ejemplo: ['name', 'email'] para limitar la búsqueda.
   */
  @Input() searchableKeys?: string[];

  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();

  displayedColumns: string[] = [];
  dataSource = new MatTableDataSource<any>([]);

  searchCtrl = new FormControl<string>('', { nonNullable: true });

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit() {
    this.displayedColumns = this.columns.map(c => c.key).concat('actions');
    this.dataSource = new MatTableDataSource(this.data ?? []);

    // Filtro personalizado: case-insensitive y sin acentos, sobre columnas elegidas
    const keys = this.searchableKeys?.length ? this.searchableKeys : this.columns.map(c => c.key);
    this.dataSource.filterPredicate = (row, rawFilter) => {
      const term = normalize(rawFilter);
      if (!term) return true;
      return keys.some(k => normalize(safeGet(row, k)).includes(term));
    };

    // Suscribir al input con debounce
    this.searchCtrl.valueChanges
      .pipe(startWith(this.searchCtrl.value), debounceTime(200), distinctUntilChanged())
      .subscribe(value => {
        this.dataSource.filter = value?.trim() ?? '';
        if (this.dataSource.paginator) this.dataSource.paginator.firstPage();
      });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  // Importante: refrescar cuando cambien los @Input
  ngOnChanges(changes: SimpleChanges) {
    if (changes['columns']) {
      this.displayedColumns = this.columns.map(c => c.key).concat('actions');
    }
    if (changes['data']) {
      this.dataSource.data = this.data ?? [];
      if (this.paginator) this.dataSource.paginator = this.paginator;
    }
  }

  onEdit(row: any) { this.edit.emit(row); }
  onDelete(row: any) {
  Swal.fire({
    title: '¿Estás seguro?',
    text: 'Esta acción no se puede deshacer',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#3085d6',
    confirmButtonText: 'Sí, eliminar',
    cancelButtonText: 'Cancelar'
  }).then((result) => {
    if (result.isConfirmed) {
      this.delete.emit(row);
    }
  });
}
}

/* Helpers */
function normalize(value: any): string {
  return String(value ?? '')
    .toLowerCase()
    .normalize('NFD')               // separa acentos
    .replace(/\p{Diacritic}/gu, ''); // quita acentos
}

function safeGet(obj: any, path: string): string {
  // Soporta 'user.name' si algún día mandas keys con anidación
  return path.split('.').reduce((acc, key) => (acc ? acc[key] : undefined), obj) ?? '';
}
