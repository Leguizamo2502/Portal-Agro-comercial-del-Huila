import { Injectable } from '@angular/core';
import { GenericService } from '../../../security/services/generic/generic.service';
import { CategoryRegistertModel, CategorySelectModel } from '../../models/category/category.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CategoryService extends GenericService<CategorySelectModel,CategoryRegistertModel> {

  constructor(http: HttpClient) {
    super(http,'Category')
   }
}
