import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Empresa } from '../models/empresa';
import { API } from '../../../core/config/api.config';
import { Observable, shareReplay, tap } from 'rxjs';
import { PagedResult } from '../../../shared/models/paged-result';

@Injectable({
  providedIn: 'root',
})
export class EmpresaService {
  private http = inject(HttpClient);
  private cache = new Map<string, Observable<PagedResult<Empresa>>>();
  private empresaByIdCache = new Map<string, Observable<Empresa>>();

  //#region Operações CRUD
  public filtrar(filtro: any): Observable<PagedResult<Empresa>> {
    const key = JSON.stringify(filtro);

    if (!this.cache.has(key)) {

      let params = new HttpParams();

      Object.keys(filtro).forEach(key => {
        const value = filtro[key];

        if (value !== null && value !== '') {
          params = params.set(key, value);
        }
      });

      const request$ = this.http
        .get<PagedResult<Empresa>>(API.endpoints.empresa, { params })
        .pipe(shareReplay(1));

      this.cache.set(key, request$);
    }

    return this.cache.get(key)!;
  }

  public getById(id: string): Observable<Empresa> {
    if (!this.empresaByIdCache.has(id)) {
      const request$ = this.http
        .get<Empresa>(`${API.endpoints.empresa}/${id}`)
        .pipe(shareReplay({
          bufferSize: 1,
          refCount: false
        }));

      this.empresaByIdCache.set(id, request$);
    }

    return this.empresaByIdCache.get(id)!;
  }

  public create(empresa: Empresa): Observable<Empresa> {
    return this.http
      .post<Empresa>(API.endpoints.empresa, empresa)
      .pipe(tap(() => this.limparCache()));
  }

  public update(id: string, empresa: Empresa): Observable<Empresa> {
    return this.http
      .put<Empresa>(`${API.endpoints.empresa}/${id}`, empresa)
      .pipe(tap(() => this.limparCache()));
  }

  public delete(id: string): Observable<void> {
    return this.http
      .delete<void>(`${API.endpoints.empresa}/${id}`)
      .pipe(tap(() => this.limparCache()));
  }

  public ativarEmpresa(id: string): Observable<void>{
    return this.http
      .patch<void>(`${API.endpoints.empresa}/${id}/ativar`, {})
      .pipe(tap(() => this.limparCache()));
  }
  //#endregion

  //#region Métodos auxiliares
  private limparCache(): void {
    this.cache.clear();
    this.empresaByIdCache.clear();
  }
}