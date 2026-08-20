import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProgressService {
  private base = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getProgress(courseId: string): Observable<any> {
    return this.http.get(`${this.base}/courses/${courseId}/progress`);
  }

  markAsCompleted(courseId: string, lessonId: string): Observable<any> {
    return this.http.post(`${this.base}/courses/${courseId}/progress/lessons/${lessonId}`, {});
  }
}
