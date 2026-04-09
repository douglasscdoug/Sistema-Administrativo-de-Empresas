import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Usuario } from '../models/usuario';
import { API } from '../../../core/config/api.config';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private http = inject(HttpClient);

  public getAll(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(API.endpoints.usuario);
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
