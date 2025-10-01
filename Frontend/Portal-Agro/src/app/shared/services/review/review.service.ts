import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { ReviewRegisterModel, ReviewSelectModel } from '../../models/product/product.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl + 'Review';

  constructor() { }
  
  getReviewByProduct(productId:number):Observable<ReviewSelectModel[]> {
    return this.http.get<ReviewSelectModel[]>(`${this.baseUrl}/by-product/${productId}`);
  }
  createReview(data: ReviewRegisterModel): Observable<ReviewSelectModel> {
    return this.http.post<ReviewSelectModel>(`${this.baseUrl}/create`, data);
  }

  deleteReview(reviewId: number):Observable<any>{ 
    return this.http.delete<any>(`${this.baseUrl}/${reviewId}`);
  }


}
