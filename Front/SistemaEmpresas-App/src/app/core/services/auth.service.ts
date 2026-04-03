import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API } from '../config/api.config';
import { tap } from 'rxjs/internal/operators/tap';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private http = inject(HttpClient);
  
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
    const token = this.getToken();
    if (!token) return false;
    
    if (this.isTokenExpired()) {
      this.logout();
      return false;
    }
    
    return true;
  }

  getTokenExpiration(): number | null {
    const token = this.getToken();
    if (!token) return null;
    
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp;
  }

  isTokenExpired(): boolean { 
    const exp = this.getTokenExpiration();
    if (!exp) return true;
    
    const now = Math.floor(Date.now() / 1000);
    return exp < (now - 60); // Considera o token expirado 1 minuto antes para evitar problemas de sincronização
  }
}
