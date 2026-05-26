import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay, tap } from 'rxjs';
import { Usuario } from '../models/usuario';
import { API } from '../../../core/config/api.config';
import { PagedResult } from '../../../shared/models/paged-result';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private http = inject(HttpClient);
  private cache = new Map<string, Observable<PagedResult<Usuario>>>();
  private usuarioByIdCache = new Map<string, Observable<Usuario>>();

  //#region Operações CRUD
  public filtrar(filtro: any): Observable<PagedResult<Usuario>> {
    const key = JSON.stringify(filtro);

    if (!this.cache.has(key)) {
      let params = new HttpParams;
  
      Object.keys(filtro).forEach(key => {
        const value = filtro[key];
  
        if (value !== null && value !== '') {
          params = params.set(key, value);
        }
      });
      
      const request$ = this.http
        .get<PagedResult<Usuario>>(API.endpoints.usuario, { params })
        .pipe(shareReplay(1));
      
      this.cache.set(key, request$);
    }
  
    return this.cache.get(key)!;
  }

  public getById(id: string): Observable<Usuario> {
    if (!this.usuarioByIdCache.has(id)) {
      const request$ = this.http
        .get<Usuario>(`${API.endpoints.usuario}/${id}`)
        .pipe(shareReplay(1));
      
      this.usuarioByIdCache.set(id, request$);
    }

    return this.usuarioByIdCache.get(id)!;
  }

  public create(usuario: Usuario): Observable<Usuario> {
    return this.http
      .post<Usuario>(API.endpoints.usuario, usuario)
      .pipe(tap(() => this.limparCache()));
  }

  public update(id: string, usuario: Usuario): Observable<Usuario> {
    return this.http
      .patch<Usuario>(`${API.endpoints.usuario}/${id}`, usuario)
      .pipe(tap(() => this.limparCache()));
  }

  public ativarUsuario(id: string): Observable<void> {
    return this.http
      .patch<void>(`${API.endpoints.usuario}/${id}/ativar`, {})
      .pipe(tap(() => this.limparCache()));
  }

  public delete(id: string): Observable<void> {
    return this.http
      .delete<void>(`${API.endpoints.usuario}/${id}`)
      .pipe(tap(() => this.limparCache()));
  }
  //#endregion

  //#region Métodos auxiliares
  private limparCache(): void {
    this.cache.clear();
    this.usuarioByIdCache.clear();
  }
}
