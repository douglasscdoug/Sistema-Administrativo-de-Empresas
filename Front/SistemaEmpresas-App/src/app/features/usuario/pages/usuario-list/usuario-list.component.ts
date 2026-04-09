import { ChangeDetectorRef, Component, inject, OnInit, TemplateRef } from '@angular/core';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario } from '../../models/usuario';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, RouterLink } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-usuario-list',
  imports: [RouterLink],
  templateUrl: './usuario-list.component.html',
  styleUrl: './usuario-list.component.scss',
})
export class UsuarioListComponent implements OnInit{
  private usuarioService = inject(UsuarioService);
  private spinner = inject(NgxSpinnerService);
  private cdr = inject(ChangeDetectorRef);
  private router = inject(Router);
  private modalService = inject(BsModalService);
  private toaster = inject(ToastrService);

  public usuarios: Usuario[] = [];
  public usuarioId: string = '';
  public modalRef?: BsModalRef;

  ngOnInit(): void {
    this.load();
  }

  public load(): void { 
    this.spinner.show();
    this.usuarioService.getAll().subscribe((usuarios) => {
      this.usuarios = usuarios;
      this.cdr.detectChanges();
    }).add(() => this.spinner.hide());
  }

  public usuarioDetalhe(id: string): void {
    this.router.navigate([`/usuarios/${id}`]);
  }

  public openModal(event: any, template: TemplateRef<void>, usuarioId: string): void {
      event.stopPropagation();
      this.usuarioId = usuarioId;
      this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
    }

  public confirm(): void{
    this.modalRef?.hide();
    this.spinner.show();

    this.usuarioService.delete(this.usuarioId).subscribe({
      next: () => {
        this.toaster.success('Usuário deletado com sucesso!', 'Sucesso');
        this.load();
      },
      error: () => {
        this.toaster.error('Erro ao deletar usuário!', 'Erro');
      }
    }).add(() => this.spinner.hide());
  }

  public declined(): void{
    this.modalRef?.hide();
  }
}