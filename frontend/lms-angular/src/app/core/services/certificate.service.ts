import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CertificateService {
  private url = `${environment.apiUrl}/certificates`;
  constructor(private http: HttpClient) {}

  getMyCertificate(courseId: string): Observable<any> {
    return this.http.get(`${this.url}/my/${courseId}`);
  }

  getMyCertificates(): Observable<any[]> {
    return this.http.get<any[]>(`${this.url}/my`);
  }

  validate(code: string): Observable<any> {
    return this.http.get(`${this.url}/validate/${code}`);
  }
}
