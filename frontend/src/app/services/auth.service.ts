import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, map, BehaviorSubject } from 'rxjs';
import { AuthResponseDto, LoginDto, RegisterDto } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private readonly BASE_URL = 'http://localhost:5000/api/auth';
  private readonly TOKEN_KEY = 'marshtech_auth_token';
  private readonly USER_KEY = 'marshtech_user_data';

  private currentUserSubject = new BehaviorSubject<AuthResponseDto | null>(this.getUserFromStorage());
  currentUser$ = this.currentUserSubject.asObservable();

  login(credentials: LoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.BASE_URL}/login`, credentials).pipe(
      tap(response => this.handleAuthentication(response))
    );
  }

  register(user: RegisterDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.BASE_URL}/register`, user).pipe(
      tap(response => this.handleAuthentication(response))
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getCurrentUser(): AuthResponseDto | null {
    return this.currentUserSubject.value;
  }

  hasPrivilegedRole(): boolean {
    const user = this.getCurrentUser();
    if (!user) return false;
    return ['Hardware Specialist', 'Project Manager'].includes(user.role);
  }

  private handleAuthentication(response: AuthResponseDto): void {
    localStorage.setItem(this.TOKEN_KEY, response.token);
    localStorage.setItem(this.USER_KEY, JSON.stringify(response));
    this.currentUserSubject.next(response);
  }

  private getUserFromStorage(): AuthResponseDto | null {
    const userData = localStorage.getItem(this.USER_KEY);
    return userData ? JSON.parse(userData) : null;
  }
}
