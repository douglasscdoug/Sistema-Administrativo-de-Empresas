import { ChangeDetectorRef, Component, inject, OnInit, TemplateRef } from '@angular/core';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario } from '../../models/usuario';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, RouterLink } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { DateFormatPipe } from "../../../../shared/pipes/date-format-pipe";
import { debounceTime, finalize } from 'rxjs';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { HasRoleDirective } from '../../../../shared/directives/has-role.directive';

@Component({
  selector: 'app-usuario-list',
  imports: [RouterLink, DateFormatPipe, CommonModule, PaginationModule, ReactiveFormsModule, HasRoleDirective],
  templateUrl: './usuario-list.component.html',
  styleUrl: './usuario-list.component.scss',
})
export class UsuarioListComponent implements OnInit {
  private usuarioService = inject(UsuarioService);
  private spinner = inject(NgxSpinnerService);
  private cdr = inject(ChangeDetectorRef);
  private router = inject(Router);
  private modalService = inject(BsModalService);
  private toaster = inject(ToastrService);
  private fb = inject(FormBuilder);

  public usuarios: Usuario[] = [];
  public usuarioId: string = '';
  public modalRef?: BsModalRef;
  //Paginação
  public total = 0;
  public page = 1;
  public pageSize = 10;
  public loading = false;
  public orderBy =  '';
  public desc = false;

  public filtroForm = this.fb.group({
    nome: [''],
    email: [''],
    ativo: [null]
  }, { updateOn: 'change' })

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

    this.usuarioService.filtrar(filtro)
      .pipe(finalize(() => {
        this.loading = false;
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: (res) => {
          this.usuarios = res.data;
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

  public usuarioDetalhe(id: string): void {
    this.router.navigate([`/usuarios/${id}`]);
  }

  public openModal(event: any, template: TemplateRef<void>, usuarioId: string): void {
    event.stopPropagation();
    this.usuarioId = usuarioId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.usuarioService.delete(this.usuarioId)
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe({
        next: () => {
          this.toaster.success('Usuário deletado com sucesso!', 'Sucesso');
          this.buscar();
        }
      });
  }

  public declined(): void {
    this.modalRef?.hide();
  }
}