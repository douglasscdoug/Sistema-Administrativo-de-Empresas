import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UsuarioService } from '../../services/usuario.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs';
import { FormErrorService } from '../../../../core/services/form-error.service';

@Component({
  selector: 'app-usuario-detalhe',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './usuario-detalhe.component.html',
  styleUrl: './usuario-detalhe.component.scss',
})
export class UsuarioDetalheComponent implements OnInit {
  private fb = inject(FormBuilder);
  private usuarioService = inject(UsuarioService);
  private spinner = inject(NgxSpinnerService);
  private router = inject(Router);
  private toaster = inject(ToastrService);
  private actvatedRoute = inject(ActivatedRoute);
  private formErrorService = inject(FormErrorService);

  public usuarioId?: string | null = null;
  public isEditMode: boolean = !!this.usuarioId;

  public get f(): any {
    return this.form.controls;
  };

  public form: FormGroup = this.fb.nonNullable.group({
    nome: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, Validators.minLength(6)]]
  });

  ngOnInit(): void {
    this.actvatedRoute.paramMap.subscribe(params => {
      this.usuarioId = params.get('id');
      this.isEditMode = !!this.usuarioId;

      if (this.isEditMode) {
        this.load(this.usuarioId!);
      }
    });
  }

  public load(id: string): void {
    this.usuarioService.getById(id).subscribe((res) => {
      this.form.patchValue(res);
    })
  }

  public isInvalid(path: string): boolean {
    const control = this.form.get(path);
    return !!(control && control.invalid && (control.touched || control.dirty));
  }

  public salvar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.spinner.show();

    const data = this.form.value

    const request = this.isEditMode
      ? this.usuarioService.update(this.usuarioId!, data)
      : this.usuarioService.create(data);

    request
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe({
        next: () => {
          this.toaster.success(`Usuário ${this.isEditMode ? 'Atualizado' : 'Adicionado'} com sucesso`, 'Sucesso');
          this.router.navigate(['/usuarios']);
        },
        error: (err) => this.formErrorService.aplicarErros(this.form, err)
      });
  }

  public voltar(): void {
    this.router.navigate(['/usuarios']);
  }
}