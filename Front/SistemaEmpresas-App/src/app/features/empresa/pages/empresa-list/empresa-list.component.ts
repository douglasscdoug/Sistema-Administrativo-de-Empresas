import { ChangeDetectorRef, Component, inject, OnInit, TemplateRef } from '@angular/core';
import { EmpresaService } from '../../services/empresa.service';
import { Empresa } from '../../models/empresa';
import { Router, RouterLink } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-empresa-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './empresa-list.component.html',
  styleUrl: './empresa-list.component.scss',
})
export class EmpresaListComponent implements OnInit{

  private empresaService = inject(EmpresaService);
  private router = inject(Router);
  private modalService = inject(BsModalService);
  private spinner = inject(NgxSpinnerService);
  private toaster = inject(ToastrService);
  private cdr = inject(ChangeDetectorRef);

  public modalRef?: BsModalRef;
  public mensagem?: string;
  public empresaId: string = '';
  public empresas: Empresa[] = [];

  
  ngOnInit(): void {
    this.load();
  }

  public load(): void {
    this.spinner.show();
    this.empresaService.getAll().subscribe((empresas) => {
      this.empresas = empresas;
      this.cdr.detectChanges();
    }).add(() => this.spinner.hide());
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

    this.empresaService.delete(this.empresaId).subscribe({
      next: () => {
        this.toaster.success('Empresa deletada com sucesso!', 'Sucesso');
        this.load();
       },
      error: () => {
        this.toaster.error('Erro ao deletar empresa. Tente novamente mais tarde.', 'Erro');
      }
    }).add(() => this.spinner.hide());
  }

  public declined(): void {
    this.modalRef?.hide();
  }
}