import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API } from '../config/api.config';
import { tap } from 'rxjs/internal/operators/tap';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable } from 'rxjs';
import { Usuario } from '../../features/usuario/models/usuario';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private http = inject(HttpClient);
  private tokenKey = 'token';
  private userSubject = new BehaviorSubject<Usuario | null>(null);

  public user$ = this.userSubject.asObservable();

  public get user() {
    return this.userSubject.value;
  }

  // Métodos CRUD
  public getMe(): Observable<Usuario>{
    return this.http.get<Usuario>(`${API.endpoints.usuario}/me`);
  }

  public login(credentials: any) {
    return this.http.post<any>(API.endpoints.auth.login, credentials).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
        localStorage.setItem('refreshToken', response.refreshToken);
      })
    );
  }

  public logout() { 
    const refreshToken = this.getRefreshToken();

    if (refreshToken) {
      this.http.post(API.endpoints.auth.logout, { refreshToken }).subscribe();
    }

    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem('refreshToken');
  }
  
  public refreshToken() {
    return this.http.post<any>(API.endpoints.auth.refresh, { refreshToken: this.getRefreshToken() });
  }

  // Métodos auxiliares
  public getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  public getRole(): string | null {
    return this.user?.role ?? null;
  } 

  public getToken(): string | null { 
    return localStorage.getItem(this.tokenKey);
  }

  public getTokenExpiration(): number | null {
    const token = this.getToken();
    if (!token) return null;
    
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp;
  }

  public getUser(): any {
    const token = this.getToken();
    if (!token) return null;

    return jwtDecode(token);
  }

  public isAdmin(): boolean {
    const token = this.getToken();

    if (!token) return false;

    const payload = JSON.parse(atob(token.split('.')[1]));

    return payload.role === 'Administrador';
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

  public isTokenExpired(): boolean { 
    const exp = this.getTokenExpiration();
    if (!exp) return true;
    
    const now = Math.floor(Date.now() / 1000);
    return exp < (now - 60);
  }

  public loadUser() {
    if (this.isAuthenticated()) {
      this.getMe().subscribe(user => this.setUser(user));
    }
  }

  public setRefreshToken(token: string) {
    localStorage.setItem('refreshToken', token);
  }

  public setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  public setUser(user: Usuario) {
    this.userSubject.next(user);
  }
}
