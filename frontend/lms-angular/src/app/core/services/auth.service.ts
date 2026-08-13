import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest, User } from '../models/auth.models';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'lms_access_token';
  private readonly REFRESH_KEY = 'lms_refresh_token';
  private readonly USER_KEY = 'lms_user';
  private _user = signal<User | null>(this.loadUser());
  user = this._user.asReadonly();
  isAuthenticated = computed(() => this._user() !== null);
  isAdmin = computed(() => this._user()?.role === 'Admin');

  constructor(private http: HttpClient, private router: Router) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, request)
      .pipe(tap(r => this.handleAuthResponse(r)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, request)
      .pipe(tap(r => this.handleAuthResponse(r)));
  }

  logout(): void {
    const rt = this.getRefreshToken();
    if (rt) this.http.post(`${environment.apiUrl}/auth/revoke`, { refreshToken: rt }).subscribe();
    this.clearStorage();
    this.router.navigate(['/login']);
  }

  refreshToken(): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/refresh`, { refreshToken: this.getRefreshToken() })
      .pipe(tap(r => this.handleAuthResponse(r)));
  }

  getToken(): string | null { return localStorage.getItem(this.TOKEN_KEY); }
  getRefreshToken(): string | null { return localStorage.getItem(this.REFRESH_KEY); }

  private handleAuthResponse(r: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, r.accessToken);
    localStorage.setItem(this.REFRESH_KEY, r.refreshToken);
    const user: User = { userName: r.userName, email: r.email, role: r.role };
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
    this._user.set(user);
  }

  private clearStorage(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_KEY);
    localStorage.removeItem(this.USER_KEY);
    this._user.set(null);
  }

  private loadUser(): User | null {
    const s = localStorage.getItem(this.USER_KEY);
    return s ? JSON.parse(s) : null;
  }
}
