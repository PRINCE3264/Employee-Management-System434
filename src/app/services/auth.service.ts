// import { Injectable, inject } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import { Router } from '@angular/router';
// import { JwtHelperService } from '@auth0/angular-jwt';
// import { environment } from '../../environments/environment.development';
// import { AuthTokenDto } from '../types/auth';
// import { HttpHeaders } from '@angular/common/http';


// @Injectable({ providedIn: 'root' })
// export class AuthService {
//   http = inject(HttpClient);
//   router = inject(Router);
//   jwtHelper = new JwtHelperService();

//   Login(email: string, password: string) {
//     return this.http.post<AuthTokenDto>(environment.apiUrlss, { email, password });
//   }

//   // ✅ Save full user object (token + role + id + email)
//   saveToken(user: AuthTokenDto) {
//     localStorage.setItem('auth', JSON.stringify(user));
//   }

//   // ✅ Get token from stored object
//   getToken(): string | null {
//     const auth = localStorage.getItem('auth');
//     return auth ? JSON.parse(auth).token : null;
//   }

//   // ✅ Remove token and user
//   logout() {
//     localStorage.removeItem('auth');
//     this.router.navigate(['/login']);
//   }

//   // ✅ Check if token exists and is valid
//   get isLoggedIn(): boolean {
//     const token = this.getToken();
//     return !!token && !this.jwtHelper.isTokenExpired(token);
//   }

//   // ✅ Get user role from stored auth
//   get userRole(): string | null {
//     const auth = localStorage.getItem('auth');
//     return auth ? JSON.parse(auth).role : null;
//   }

//   // ✅ Role-check helpers
//   get isAdmin(): boolean {
//     return this.userRole === 'Admin';
//   }

//   get isEmployee(): boolean {
//     return this.userRole === 'Employee';
//   }
//   get authDetail(): AuthTokenDto | null {
//     if (!this.isLoggedIn) return null;
//     let token: AuthTokenDto = JSON.parse(localStorage.getItem('auth')!)
//     return token

//   }


//   getProfile() {
//     const token = JSON.parse(localStorage.getItem('auth') || '{}')?.token || '';
//     const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
//     return this.http.get(`${environment.apiUrlsss}`, { headers });
//   }

//   updateProfile(data: any) {
//     const token = JSON.parse(localStorage.getItem('auth') || '{}')?.token || '';
//     const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
//     return this.http.post(`${environment.apiUrlsss}`, data, { headers });
//   }


// }



import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment.development';
import { AuthTokenDto } from '../types/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private jwtHelper = new JwtHelperService();

  // ✅ Login API
  Login(email: string, password: string) {
    return this.http.post<AuthTokenDto>(environment.apiUrlss, { email, password });
  }

  // ✅ Save full user object (token, role, etc.)
  saveToken(user: AuthTokenDto) {
    localStorage.setItem('auth', JSON.stringify(user));
  }

  // ✅ Get token string from localStorage
  getToken(): string | null {
    const auth = localStorage.getItem('auth');
    return auth ? JSON.parse(auth).token : null;
  }

  // ✅ Logout and clear token
  logout() {
    localStorage.removeItem('auth');
    this.router.navigate(['/login']);
  }

  // ✅ Check if user is logged in
  get isLoggedIn(): boolean {
    const token = this.getToken();
    return !!token && !this.jwtHelper.isTokenExpired(token);
  }

  // ✅ Get user role from stored auth
  get userRole(): string | null {
    const auth = localStorage.getItem('auth');
    return auth ? JSON.parse(auth).role : null;
  }

  // ✅ Get entire auth object (if logged in)
  get authDetail(): AuthTokenDto | null {
    if (!this.isLoggedIn) return null;
    return JSON.parse(localStorage.getItem('auth')!);
  }

  // ✅ Role checks as getters (used as: this.authService.isAdmin)
  get isAdmin(): boolean {
    return this.userRole === 'Admin';
  }

  get isEmployee(): boolean {
    return this.userRole === 'Employee';
  }

  // ✅ Role checks as methods (used as: this.authService.isAdminRole())
  isAdminRole(): boolean {
    return this.userRole === 'Admin';
  }

  isEmployeeRole(): boolean {
    return this.userRole === 'Employee';
  }

  // ✅ Get user profile API
  getProfile() {
    const token = this.getToken() || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get(`${environment.apiUrlsss}`, { headers });
  }

  // ✅ Update profile API
  updateProfile(data: any) {
    const token = this.getToken() || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post(`${environment.apiUrlsss}`, data, { headers });
  }
}
