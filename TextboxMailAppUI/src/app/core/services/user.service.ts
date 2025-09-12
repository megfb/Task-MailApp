import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, EMPTY } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:7135/api/User';

  constructor(private http: HttpClient) {}

  //token ı çağır
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token') || '';
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  //user update
  updateUser(user: UserModel): Observable<any> {
    return this.http.put(`${this.apiUrl}/Update`, user, { headers: this.getAuthHeaders() }).pipe(
      tap(() => console.log('Kullanıcı güncellendi')),
      catchError(err => {
        console.error('updateUser hatası:', err);
        return EMPTY;
      })
    );
  }
}
