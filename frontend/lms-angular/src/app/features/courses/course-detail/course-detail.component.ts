import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseService } from '../../../core/services/course.service';
import { ModuleLessonService } from '../../../core/services/module-lesson.service';
import { AuthService } from '../../../core/services/auth.service';
import { Course } from '../../../core/models/course.models';
import { Module } from '../../../core/models/module.models';
import { LoadingComponent } from '../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-course-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule, LoadingComponent],
  templateUrl: './course-detail.component.html'
})
export class CourseDetailComponent implements OnInit {
  course = signal<Course | null>(null);
  modules = signal<Module[]>([]);
  loading = signal(true);
  courseId = '';
  showModuleForm = signal(false);
  showLessonForm = signal<string | null>(null);
  moduleForm: FormGroup;
  lessonForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private moduleLessonService: ModuleLessonService,
    public authService: AuthService,
    private fb: FormBuilder
  ) {
    this.moduleForm = this.fb.group({ title: ['', [Validators.required, Validators.minLength(3)]], order: [1, [Validators.required, Validators.min(1)]] });
    this.lessonForm = this.fb.group({ title: ['', [Validators.required, Validators.minLength(3)]], content: ['', [Validators.required, Validators.minLength(10)]], order: [1, [Validators.required, Validators.min(1)]] });
  }

  ngOnInit(): void {
    this.courseId = this.route.snapshot.params['id'];
    this.courseService.getById(this.courseId).subscribe(c => this.course.set(c));
    this.loadModules();
  }

  loadModules(): void {
    this.loading.set(true);
    this.moduleLessonService.getModules(this.courseId).subscribe({
      next: (m) => { this.modules.set(m); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  createModule(): void {
    if (this.moduleForm.invalid) return;
    this.moduleLessonService.createModule(this.courseId, this.moduleForm.value).subscribe(() => {
      this.showModuleForm.set(false); this.moduleForm.reset({ order: 1 }); this.loadModules();
    });
  }

  createLesson(moduleId: string): void {
    if (this.lessonForm.invalid) return;
    this.moduleLessonService.createLesson(this.courseId, moduleId, this.lessonForm.value).subscribe(() => {
      this.showLessonForm.set(null); this.lessonForm.reset({ order: 1 }); this.loadModules();
    });
  }

  deleteModule(moduleId: string): void {
    if (!confirm('Excluir modulo?')) return;
    this.moduleLessonService.deleteModule(this.courseId, moduleId).subscribe(() => this.loadModules());
  }
}
