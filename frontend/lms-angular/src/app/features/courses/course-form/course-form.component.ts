import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { CourseService } from '../../../core/services/course.service';

@Component({
  selector: 'app-course-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './course-form.component.html'
})
export class CourseFormComponent implements OnInit {
  form: FormGroup;
  saving = signal(false);
  error = signal('');
  isEdit = false;
  courseId = '';

  constructor(private fb: FormBuilder, private courseService: CourseService, private router: Router, private route: ActivatedRoute) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(2000)]]
    });
  }

  ngOnInit(): void {
    this.courseId = this.route.snapshot.params['id'];
    this.isEdit = !!this.courseId && this.courseId !== 'new';
    if (this.isEdit) {
      this.courseService.getById(this.courseId).subscribe(c => this.form.patchValue({ title: c.title, description: c.description }));
    }
  }

  onSubmit(): void {
    if (this.form.invalid || this.saving()) return;
    this.saving.set(true);
    const action = this.isEdit ? this.courseService.update(this.courseId, this.form.value) : this.courseService.create(this.form.value);
    action.subscribe({
      next: (c) => this.router.navigate(['/courses', c.id]),
      error: (err: any) => { this.error.set(err.error?.message || 'Erro ao salvar.'); this.saving.set(false); }
    });
  }
}
