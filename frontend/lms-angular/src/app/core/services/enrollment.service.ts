import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class EnrollmentService {
  private url = `${environment.apiUrl}/enrollments`;
  constructor(private http: HttpClient) {}
  enroll(courseId: string): Observable<any> { return this.http.post(`${this.url}/${courseId}`, {}); }
  getMyEnrollments(): Observable<any[]> { return this.http.get<any[]>(`${this.url}/my`); }
  unenroll(courseId: string): Observable<void> { return this.http.delete<void>(`${this.url}/${courseId}`); }
}
