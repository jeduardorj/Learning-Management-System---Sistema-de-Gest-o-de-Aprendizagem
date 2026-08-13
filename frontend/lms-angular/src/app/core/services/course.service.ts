import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Course, CourseRequest } from '../models/course.models';
import { PagedResult, CourseFilter } from '../models/pagination.models';

@Injectable({ providedIn: 'root' })
export class CourseService {
  private url = `${environment.apiUrl}/courses`;
  constructor(private http: HttpClient) {}

  getAll(filter: CourseFilter): Observable<PagedResult<Course>> {
    let params = new HttpParams().set('page', filter.page).set('pageSize', filter.pageSize);
    if (filter.search) params = params.set('search', filter.search);
    if (filter.isActive !== undefined) params = params.set('isActive', filter.isActive);
    return this.http.get<PagedResult<Course>>(this.url, { params });
  }

  getById(id: string): Observable<Course> { return this.http.get<Course>(`${this.url}/${id}`); }
  create(r: CourseRequest): Observable<Course> { return this.http.post<Course>(this.url, r); }
  update(id: string, r: CourseRequest): Observable<Course> { return this.http.put<Course>(`${this.url}/${id}`, r); }
  delete(id: string): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
  activate(id: string): Observable<Course> { return this.http.patch<Course>(`${this.url}/${id}/activate`, {}); }
  deactivate(id: string): Observable<Course> { return this.http.patch<Course>(`${this.url}/${id}/deactivate`, {}); }
}
