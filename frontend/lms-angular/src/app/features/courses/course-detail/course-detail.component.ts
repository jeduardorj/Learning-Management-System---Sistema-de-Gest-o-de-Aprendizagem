import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseService } from '../../../core/services/course.service';
import { ModuleLessonService } from '../../../core/services/module-lesson.service';
import { EnrollmentService } from '../../../core/services/enrollment.service';
import { ProgressService } from '../../../core/services/progress.service';
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
  progress = signal<any>(null);
  loading = signal(true);
  enrolling = signal(false);
  enrolled = signal(false);
  courseId = '';
  showModuleForm = signal(false);
  showLessonForm = signal<string | null>(null);
  moduleForm: FormGroup;
  lessonForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private moduleLessonService: ModuleLessonService,
    private enrollmentService: EnrollmentService,
    private progressService: ProgressService,
    public authService: AuthService,
    private fb: FormBuilder
  ) {
    this.moduleForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      order: [1, [Validators.required, Validators.min(1)]]
    });
    this.lessonForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      content: ['', [Validators.required, Validators.minLength(10)]],
      order: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.courseId = this.route.snapshot.params['id'];
    this.courseService.getById(this.courseId).subscribe(c => this.course.set(c));
    this.loadModules();
    this.loadProgress();
  }

  loadModules(): void {
    this.loading.set(true);
    this.moduleLessonService.getModules(this.courseId).subscribe({
      next: (m) => { this.modules.set(m); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  loadProgress(): void {
    this.progressService.getProgress(this.courseId).subscribe({
      next: (p) => { this.progress.set(p); this.enrolled.set(true); },
      error: () => this.enrolled.set(false)
    });
  }

  enroll(): void {
    this.enrolling.set(true);
    this.enrollmentService.enroll(this.courseId).subscribe({
      next: () => { this.enrolled.set(true); this.loadProgress(); this.enrolling.set(false); },
      error: (err: any) => { alert(err.error?.message || 'Erro ao matricular.'); this.enrolling.set(false); }
    });
  }

  markAsCompleted(lessonId: string): void {
    this.progressService.markAsCompleted(this.courseId, lessonId).subscribe(() => this.loadProgress());
  }

  isLessonCompleted(lessonId: string): boolean {
    const p = this.progress();
    if (!p?.progresses) return false;
    return p.progresses.some((x: any) => x.lessonId === lessonId && x.completed);
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
