// import { Injectable, inject } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Observable, forkJoin, map } from 'rxjs';
// import { AnalyticService } from '../../shared/services/analytics/analytic.service';
// import { OrderService } from '../../features/products/services/order/order.service';

// @Injectable({
//     providedIn: 'root'
// })
// export class DashboardService {
//   private http = inject(HttpClient);
//   private analyticService = inject(AnalyticService);
//   private orderService = inject(OrderService);

//   getSummary(): Observable<any> {

//     return forkJoin({
//       totalOrders: this.orderService.getMine().pipe(map(orders => orders.length)),
//       totalProducers: this.getTotalProducers(),
//       totalProducts: this.analyticService.getTopProducts().pipe(map(res => res.totalProducts)),
//       pendingOrders: this.orderService.getMine().pipe(map(orders => orders.filter(o => o.status === 'Pending').length))
//     }).pipe(
//       map(({ totalOrders, totalProducers, totalProducts, pendingOrders }) => ({
//         totalOrders,
//         totalProducers,
//         totalProducts,
//         pendingOrders
//       }))
//     );
//   }

//   getTopProducts(): Observable<any> {
//     return this.analyticService.getTopProducts().pipe(
//       map(res => ({
//         labels: res.items.map(item => item.productName),
//         values: res.items.map(item => item.completedOrders)
//       }))
//     );
//   }

//   getTopProducers(): Observable<any> {

//     return this.http.get('https://localhost:5001/api/dashboard/producers/top').pipe(
//       map((data: any) => ({
//         labels: data?.labels || [],
//         values: data?.values || []
//       }))
//     );
//   }

//   private getTotalProducers(): Observable<number> {

//     return new Observable(subscriber => {
//       subscriber.next(50);
//       subscriber.complete();
//     });
//   }
// }
