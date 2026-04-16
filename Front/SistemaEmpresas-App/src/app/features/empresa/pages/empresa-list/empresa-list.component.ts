import { ChangeDetectorRef, Component, inject, OnInit, TemplateRef } from '@angular/core';
import { EmpresaService } from '../../services/empresa.service';
import { Empresa } from '../../models/empresa';
import { Router, RouterLink } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { NgxMaskPipe } from 'ngx-mask';
import { debounceTime, finalize } from 'rxjs';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-empresa-list',
  imports: [CommonModule, RouterLink, NgxMaskPipe, PaginationModule, ReactiveFormsModule],
  templateUrl: './empresa-list.component.html',
  styleUrl: './empresa-list.component.scss',
})
export class EmpresaListComponent implements OnInit {

  private empresaService = inject(EmpresaService);
  private router = inject(Router);
  private modalService = inject(BsModalService);
  private spinner = inject(NgxSpinnerService);
  private toaster = inject(ToastrService);
  private cdr = inject(ChangeDetectorRef);
  private fb = inject(FormBuilder);

  public modalRef?: BsModalRef;
  public mensagem?: string;
  public empresaId: string = '';
  public empresas: Empresa[] = [];
  //Paginação
  public total = 0;
  public page = 1;
  public pageSize = 10;
  public loading = false;
  public orderBy = '';
  public desc = false;

  public filtroForm = this.fb.group({
    razaoSocial: [''],
    cnpj: [''],
    ativo: [null]
  }, { updateOn: 'change' });

  ngOnInit(): void {
    this.buscar();

    this.filtroForm.valueChanges
      .pipe(debounceTime(500))
      .subscribe(() => {
        if (this.page !== 1) {
          this.page = 1;
        }
        this.buscar();
      });
  }

  public buscar(): void {
    this.loading = true;

    const filtro = {
      ...this.filtroForm.value,
      page: this.page,
      pageSize: this.pageSize,
      orderBy: this.orderBy,
      desc: this.desc
    };

    this.empresaService.filtrar(filtro)
      .pipe(finalize(() => {
        this.loading = false;
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: (res) => {
          this.empresas = res.data;
          this.total = res.total;
          this.cdr.detectChanges();
        }
      })
  }

  public filtrar(): void {
    this.page = 1;
    this.buscar();
  }

  public onPageChange(event: any) {
    this.page = event.page;
    this.buscar();
  }

  public onPageSizeChange(event: any) {
    this.pageSize = +event.target.value;
    this.page = 1;
    this.buscar();
  }

  public ordenar(coluna: string): void {
    if (this.orderBy === coluna) {
      this.desc = !this.desc;
    } else {
      this.orderBy = coluna;
      this.desc = false;
    }

    this.page = 1;
    this.buscar();
  }

  public empresaDetalhe(id: string): void {
    this.router.navigate([`/empresas/${id}`]);
  }

  public openModal(event: any, template: TemplateRef<void>, empresaId: string): void {
    event.stopPropagation();
    this.empresaId = empresaId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.empresaService.delete(this.empresaId)
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe({
        next: () => {
          this.toaster.success('Empresa deletada com sucesso!', 'Sucesso');
          this.buscar();
        }
      });
  }

  public declined(): void {
    this.modalRef?.hide();
  }
}