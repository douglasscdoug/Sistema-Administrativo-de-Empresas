import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Usuario } from '../models/usuario';
import { API } from '../../../core/config/api.config';
import { PagedResult } from '../../../shared/models/paged-result';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private http = inject(HttpClient);

  public filtrar(filtro: any): Observable<PagedResult<Usuario>> {
      let params = new HttpParams;
  
      Object.keys(filtro).forEach(key => {
        const value = filtro[key];
  
        if (value !== null && value !== '') {
          params = params.set(key, value);
        }
      });
  
      return this.http.get<PagedResult<Usuario>>(API.endpoints.usuario, { params });
    }

  public getById(id: string): Observable<Usuario> {
    return this.http.get<Usuario>(`${API.endpoints.usuario}/${id}`);
  }

  public create(usuario: Usuario): Observable<Usuario> {
    return this.http.post<Usuario>(API.endpoints.usuario, usuario);
  }

  public update(id: string, usuario: Usuario): Observable<Usuario> {
    return this.http.patch<Usuario>(`${API.endpoints.usuario}/${id}`, usuario);
  }

  public delete(id: string): Observable<void> {
    return this.http.delete<void>(`${API.endpoints.usuario}/${id}`);
  }
}
