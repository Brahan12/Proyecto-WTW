import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastService } from '../../shared/services/toast.service';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const toast = inject(ToastService);

  return next(req).pipe(
    catchError((err) => {
      if (err.status === 401) {
        toast.showError("No autorizado. Inicie sesión nuevamente.");
      } else {
        toast.showError(err?.error?.message || "Ocurrió un error inesperado.");
      }

      return throwError(() => err);
    })
  );
};
