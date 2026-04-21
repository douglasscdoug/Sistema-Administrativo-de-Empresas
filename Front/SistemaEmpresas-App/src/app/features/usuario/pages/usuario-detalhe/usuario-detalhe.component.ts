import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UsuarioService } from '../../services/usuario.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs';
import { FormErrorService } from '../../../../core/services/form-error.service';
import { senhaValidator } from '../../../../shared/validators/senha.validator';
import { AuthService } from '../../../../core/services/auth.service';
import { HasRoleDirective } from '../../../../shared/directives/has-role.directive';

@Component({
  selector: 'app-usuario-detalhe',
  imports: [CommonModule, ReactiveFormsModule, HasRoleDirective],
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
  private authService = inject(AuthService);

  public usuarioId?: string | null = null;
  public isEditMode: boolean = !!this.usuarioId;
  public isPerfil = false;

  public get f(): any {
    return this.form.controls;
  };

  public get senhaValue(): string {
    return this.form.get('senha')?.value || '';
  }

  public get forcaSenha(): number {
    const senha = this.senhaValue;
    let score = 0;

    if (!senha) return 0;

    if (senha.length >= 8) score++;
    if (/[A-Za-z]/.test(senha)) score++;
    if (/\d/.test(senha)) score++;
    if (/[^A-Za-z0-9]/.test(senha)) score++;

    return score;
  }

  public get labelSenha(): string {
    if (this.forcaSenha <= 1) return 'Senha fraca';
    if (this.forcaSenha <= 3) return 'Senha média';
    return 'Senha forte';
  }

  public get classeSenha(): string {
    if (this.forcaSenha <= 1) return 'bg-danger';
    if (this.forcaSenha <= 3) return 'bg-warning';
    return 'bg-success';
  }

  public form: FormGroup = this.fb.nonNullable.group({
    nome: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, senhaValidator]],
    role: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.actvatedRoute.paramMap.subscribe(params => {
      this.usuarioId = params.get('id');
      this.isEditMode = !!this.usuarioId;

      this.isPerfil = this.actvatedRoute.snapshot.data['perfil'] === true;

      if (this.isEditMode) {
        this.load(this.usuarioId!);
      } else if (this.isPerfil) {
        this.loadPerfil();
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

    let request;

    if (this.isPerfil) {
      const user = this.authService.user;
      if (!user) return;
      request = this.usuarioService.update(user.id, data);
    } else if (this.isEditMode) {
      request = this.usuarioService.update(this.usuarioId!, data);
    } else {
      request = this.usuarioService.create(data);
    }

    request
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe({
        next: () => {
          this.toaster.success(this.isEditMode ? 'Usuário alterado com sucesso.' : this.isPerfil ? 'Perfil alterado com sucesso.' : 'Usuário adicionado com sucesso.', 'Sucesso');
          if (this.isEditMode) {
            this.router.navigate(['/dashboard'])
          }
          this.router.navigate(['/usuarios']);
        },
        error: (err) => this.formErrorService.aplicarErros(this.form, err)
      });
  }

  public voltar(): void {
    this.router.navigate(['/usuarios']);
  }

  private loadPerfil(): void {
    const user = this.authService.user;

    if (user) {
      this.form.patchValue(user);
    } else {
      this.authService.getMe().subscribe(user => {
        this.form.patchValue(user);
      });
    }
  }
}