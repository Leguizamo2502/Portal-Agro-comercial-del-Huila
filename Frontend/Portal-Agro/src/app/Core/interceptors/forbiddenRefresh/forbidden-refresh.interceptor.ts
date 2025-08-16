// import { HttpInterceptorFn } from '@angular/common/http';
// import { AuthState } from '../../services/auth/auth.state';
// import { catchError, switchMap } from 'rxjs';
// import { inject } from '@angular/core';

// export const  : HttpInterceptorFn = (req, next) => {
//   const auth = inject(AuthState);
//   return next(req).pipe(
//     catchError(err => {
//       if (err.status === 403) {
//         return auth.loadMe().pipe(
//           switchMap(() => next(req)), // reintentar 1 vez
//         );
//       // 
//       throw err;
//     })
//   );
// };
