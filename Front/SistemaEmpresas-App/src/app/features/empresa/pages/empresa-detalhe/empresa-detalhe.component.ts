import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Empresa } from '../../models/empresa';
import { EmpresaService } from '../../services/empresa.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, KeyValuePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs';
import { NgxMaskDirective } from 'ngx-mask';
import { telefoneValidator } from '../../../../shared/validators/telefone.validator';
import { FormErrorService } from '../../../../core/services/form-error.service';
import { cnpjValidator } from '../../../../shared/validators/cnpj.validator';


@Component({
  selector: 'app-empresa-detalhe',
  imports: [CommonModule, ReactiveFormsModule, NgxMaskDirective, ],
  templateUrl: './empresa-detalhe.component.html',
  styleUrl: './empresa-detalhe.component.scss',
})
export class EmpresaDetalheComponent implements OnInit {
  private fb = inject(FormBuilder);
  private empresaService = inject(EmpresaService);
  private activatedRoute = inject(ActivatedRoute);
  private toaster = inject(ToastrService);
  private router = inject(Router);
  private spinner = inject(NgxSpinnerService);
  private formErrorService = inject(FormErrorService)

  public empresaId?: string | null = null;
  public empresa = {} as Empresa;
  public isEditMode: boolean = !!this.empresaId;

  public get f(): any {
    return this.form.controls;
  };

  public get contato(): any {
    return this.form.get('contato') as FormGroup;
  };

  public get endereco(): any {
    return this.form.get('endereco') as FormGroup;
  };

  public form: FormGroup = this.fb.nonNullable.group({
    razaoSocial: ['', Validators.required],
    cnpj: ['', [Validators.required, cnpjValidator]],

    endereco: this.fb.nonNullable.group({
      id: [''],
      logradouro: ['', Validators.required],
      numero: ['', Validators.required],
      complemento: [''],
      bairro: ['', Validators.required],
      cidade: ['', Validators.required],
      estado: ['', Validators.required],
      cep: ['', [Validators.required, Validators.pattern(/^\d{8}$/)]]
    }),

    contato: this.fb.nonNullable.group({
      id: [''],
      nome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required, telefoneValidator]]
    })
  });

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.empresaId = params.get('id');
      this.isEditMode = !!this.empresaId;

      if (this.isEditMode)
        this.load(this.empresaId!);
    });
  }

  public somenteNumeros(valor: string): string{
    return (valor || '').replace(/\D/g, '');
  }

  public load(id: string): void {
    this.empresaService.getById(id).subscribe((res) => {
      this.form.patchValue(res);
    });
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

    const data = {
      ...this.form.value,
      ativo: true,
      contato: {
        ...this.form.value.contato,
        telefone: this.somenteNumeros(this.form.value.contato?.telefone)
      }
    };

    const request = this.isEditMode
      ? this.empresaService.update(this.empresaId!, data)
      : this.empresaService.create(data);

    request
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe({
        next: () => {
          this.toaster.success(`Empresa ${this.isEditMode ? 'atualizada' : 'criada'} com sucesso!`, 'Sucesso');
          this.router.navigate(['/empresas']);
        },
        error: (err) => this.formErrorService.aplicarErros(this.form, err)
      });

  }
  public voltar(): void {
    this.router.navigate(['/usuarios']);
  }
}