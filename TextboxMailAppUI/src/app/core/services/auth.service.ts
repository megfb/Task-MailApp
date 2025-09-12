import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, EMPTY } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { LoginModel } from '../models/login.model';
import { RegisterModel } from '../models/register.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:7135/api/Auth';

  constructor(private http: HttpClient, private router: Router) {}

  //login request
  login(LoginQuery: LoginModel): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, LoginQuery).pipe(
      tap(response => {
        const token = response.data;
        if (token) {
          //token dönerse login başarılı olur. token lokale kaydedilir.
          localStorage.setItem('token', token);
          console.log('Login başarılı, token kaydedildi:', token);
        }
      }),
      catchError(err => {
        //hata durumunda uyarı
        console.error('Login hatası:', err);
        return EMPTY;
      })
    );
  }

  //kullanıcı kaydı
  register(RegisterQuery: RegisterModel): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Register`, RegisterQuery).pipe(
      tap(response => {
        if (response.isSuccess) {
          console.log('Kullanıcı kaydı başarılı:', response);
        }
      }),
      catchError(err => {
        console.error('Register hatası:', err);
        return EMPTY;
      })
    );
  }

  //logout işlemi. token lokalden silinir.
  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['']);
  }

  // method çağırılırsa token lokal storage den token döner. oturum aktif mi değil mi kontrol et.
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
