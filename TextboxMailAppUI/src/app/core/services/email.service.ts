import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmailService {
  private apiUrl = 'https://localhost:7135/api/Email';

  constructor(private http: HttpClient) {}

  //token çağırılır.
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token') || '';
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  //query string yoluyla filtreleme
  GetMails(pageNumber: number, pageSize: number, searchTerm: string | null = null, sortBy: string | null = null, sortDesc: boolean = true): Observable<any> {
    let params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);

    if (searchTerm) params = params.set('searchTerm', searchTerm);
    if (sortBy) params = params.set('sortBy', sortBy).set('sortDesc', sortDesc.toString());

    return this.http.get(`${this.apiUrl}/GetMails`, { headers: this.getAuthHeaders(), params }).pipe(
      catchError(err => {
        console.error('GetMails hatası:', err);
        return EMPTY;
      })
    );
  }

  //id ye göre mail getir.
  GetMail(id: string): Observable<any> {
    const params = new HttpParams().set('id', id);

    return this.http.get<any>(`${this.apiUrl}/GetMail`, { headers: this.getAuthHeaders(), params }).pipe(
      catchError(err => {
        console.error('GetMail hatası:', err);
        return EMPTY;
      })
    );
  }

  //mailleri yenile.
  refreshMails(): Observable<any> {
    return this.http.post(`${this.apiUrl}/RefreshMails`, null, { headers: this.getAuthHeaders() }).pipe(
      catchError(err => {
        console.error('RefreshMails hatası:', err);
        return EMPTY;
      })
    );
  }
}
