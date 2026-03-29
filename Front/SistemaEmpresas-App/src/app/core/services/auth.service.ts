import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API } from '../config/api.config';
import { tap } from 'rxjs/internal/operators/tap';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) { }
  
  login(credentials: any) {
    return this.http.post<any>(API.endpoints.auth, credentials).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
      })
    );
  }

  logout() { 
    localStorage.removeItem('token');
  }

  getToken(): string | null { 
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean { 
    return !!this.getToken();
  }
}
