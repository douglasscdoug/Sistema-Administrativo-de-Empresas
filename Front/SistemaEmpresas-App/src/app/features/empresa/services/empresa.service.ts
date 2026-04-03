import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Empresa } from '../models/empresa';
import { API } from '../../../core/config/api.config';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EmpresaService {
  private http = inject(HttpClient);

  getAll(): Observable<Empresa[]> {
    return this.http.get<Empresa[]>(API.endpoints.empresa);
  }

  getById(id: string): Observable<Empresa> {
    return this.http.get<Empresa>(`${API.endpoints.empresa}/${id}`);
  }

  create(empresa: Empresa): Observable<Empresa> {
    return this.http.post<Empresa>(API.endpoints.empresa, empresa);
  }

  update(id: string, empresa: Empresa): Observable<Empresa> {
    return this.http.put<Empresa>(`${API.endpoints.empresa}/${id}`, empresa);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${API.endpoints.empresa}/${id}`);
  }
}
