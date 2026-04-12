import { Component, inject, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../core/services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs';
import { FormErrorService } from '../../../../core/services/form-error.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private spinner = inject(NgxSpinnerService);
  private formErrorService = inject(FormErrorService);

  public get f(): any {
    return this.form.controls;
  };

  form: FormGroup = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit() {
    if (this.form.invalid) return;
    this.spinner.show();
    this.authService.login(this.form.value)
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe({
        next: () => this.router.navigate(['/dashboard']),
        error: (err) => this.formErrorService.aplicarErros(this.form, err)
      });
  }
}