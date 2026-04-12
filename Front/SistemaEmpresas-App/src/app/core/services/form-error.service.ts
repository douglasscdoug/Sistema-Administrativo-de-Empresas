import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class FormErrorService {

  public aplicarErros(form: FormGroup, error: any) {
    if (!error?.error?.errors) return;

    const errors = error.error.errors;

    Object.keys(errors).forEach((field) => {
      const mensagens = errors[field];

      if (field === 'Erro') return;

      const path = this.converterCampoParaPath(field);

      const control = form.get(path);

      if (control) {
        control.setErrors({
          server: mensagens[0]
        });

        control.markAllAsTouched();
      }
    })
  }

  private converterCampoParaPath(field: string): string {
    return field.split('.').map(p => p.charAt(0).toLowerCase() + p.slice(1)).join('.');
  }
}