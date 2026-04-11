import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toaster = inject(ToastrService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      let mensagem = 'Erro inesperado. Tente novamente.';

      if (error.error) {
        if (typeof error.error === 'string') {
          mensagem = error.error;
        } else if (error.error.message) {
          mensagem = error.error.message;
        } else if (error.error?.errors) {
          const erros = Object.values(error.error.errors);

          mensagem = erros
            .flat()
            .join(' | ');
        } else if (error.error.title) {
          mensagem = error.error.title;
        }
      }

      toaster.error(mensagem, 'Erro');

      return throwError(() => error);
    })
  );
};