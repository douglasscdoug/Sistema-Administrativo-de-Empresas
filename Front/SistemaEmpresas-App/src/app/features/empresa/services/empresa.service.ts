import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Empresa } from '../models/empresa';
import { API } from '../../../core/config/api.config';
import { Observable } from 'rxjs';
import { PagedResult } from '../../../shared/models/paged-result';

@Injectable({
  providedIn: 'root',
})
export class EmpresaService {
  private http = inject(HttpClient);

  public filtrar(filtro: any): Observable<PagedResult<Empresa>> {
    let params = new HttpParams;

    Object.keys(filtro).forEach(key => {
      const value = filtro[key];

      if (value !== null && value !== '') {
        params = params.set(key, value);
      }
    });

    return this.http.get<PagedResult<Empresa>>(API.endpoints.empresa, { params });
  }

  public getById(id: string): Observable<Empresa> {
    return this.http.get<Empresa>(`${API.endpoints.empresa}/${id}`);
  }

  public create(empresa: Empresa): Observable<Empresa> {
    return this.http.post<Empresa>(API.endpoints.empresa, empresa);
  }

  public update(id: string, empresa: Empresa): Observable<Empresa> {
    return this.http.put<Empresa>(`${API.endpoints.empresa}/${id}`, empresa);
  }

  public delete(id: string): Observable<void> {
    return this.http.delete<void>(`${API.endpoints.empresa}/${id}`);
  }
}
