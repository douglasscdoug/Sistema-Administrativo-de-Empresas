import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API } from '../config/api.config';
import { tap } from 'rxjs/internal/operators/tap';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private http = inject(HttpClient);
  private tokenKey = 'token';
  
  public login(credentials: any) {
    return this.http.post<any>(API.endpoints.auth, credentials).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
      })
    );
  }

  public logout() { 
    localStorage.removeItem(this.tokenKey);
  }

  public getToken(): string | null { 
    return localStorage.getItem(this.tokenKey);
  }

  public getUser(): any {
    const token = this.getToken();
    if (!token) return null;

    return jwtDecode(token);
  }

  public getRole(): string | null {
    const user = this.getUser();
    if (!user) return null;

    return user['role'] || user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  }

  public isAdmin(): boolean {
    return this.getRole() === 'Administrador';
  }

  public isAuthenticated(): boolean { 
    const token = this.getToken();
    if (!token) return false;
    
    if (this.isTokenExpired()) {
      this.logout();
      return false;
    }
    
    return true;
  }

  public getTokenExpiration(): number | null {
    const token = this.getToken();
    if (!token) return null;
    
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp;
  }

  public isTokenExpired(): boolean { 
    const exp = this.getTokenExpiration();
    if (!exp) return true;
    
    const now = Math.floor(Date.now() / 1000);
    return exp < (now - 60); // Considera o token expirado 1 minuto antes para evitar problemas de sincronização
  }
}
