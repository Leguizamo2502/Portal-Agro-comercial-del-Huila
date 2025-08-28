import { Component, Input } from '@angular/core';
import { CategorySelectModel } from '../../../../features/parameters/models/category/category.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category-pill',
  imports: [CommonModule],
  templateUrl: './category-pill.component.html',
  styleUrl: './category-pill.component.css'
})
export class CategoryPillComponent {
  @Input() category!: CategorySelectModel;
  @Input() active: boolean = false;
}
