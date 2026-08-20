import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Module, ModuleRequest, Lesson, LessonRequest } from '../models/module.models';

@Injectable({ providedIn: 'root' })
export class ModuleLessonService {
  private base = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getModules(courseId: string): Observable<Module[]> { return this.http.get<Module[]>(`${this.base}/courses/${courseId}/modules`); }
  getModuleWithLessons(courseId: string, moduleId: string): Observable<Module> { return this.http.get<Module>(`${this.base}/courses/${courseId}/modules/${moduleId}`); }
  createModule(courseId: string, r: ModuleRequest): Observable<Module> { return this.http.post<Module>(`${this.base}/courses/${courseId}/modules`, r); }
  updateModule(courseId: string, moduleId: string, r: ModuleRequest): Observable<Module> { return this.http.put<Module>(`${this.base}/courses/${courseId}/modules/${moduleId}`, r); }
  deleteModule(courseId: string, moduleId: string): Observable<void> { return this.http.delete<void>(`${this.base}/courses/${courseId}/modules/${moduleId}`); }
  createLesson(courseId: string, moduleId: string, r: LessonRequest): Observable<Lesson> { return this.http.post<Lesson>(`${this.base}/courses/${courseId}/modules/${moduleId}/lessons`, r); }
  deleteLesson(courseId: string, moduleId: string, lessonId: string): Observable<void> { return this.http.delete<void>(`${this.base}/courses/${courseId}/modules/${moduleId}/lessons/${lessonId}`); }
}
