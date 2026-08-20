import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../../core/services/course.service';
import { AuthService } from '../../core/services/auth.service';
import { Course } from '../../core/models/course.models';
import { PagedResult, CourseFilter } from '../../core/models/pagination.models';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { PaginationComponent } from '../../shared/components/pagination/pagination.component';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-courses',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, LoadingComponent, PaginationComponent, ConfirmDialogComponent],
  templateUrl: './courses.component.html'
})
export class CoursesComponent implements OnInit {
  result = signal<PagedResult<Course> | null>(null);
  loading = signal(true);
  searchTerm = '';
  deleteTarget = signal<Course | null>(null);
  filter: CourseFilter = { page: 1, pageSize: 9 };

  constructor(private courseService: CourseService, public authService: AuthService) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading.set(true);
    this.courseService.getAll(this.filter).subscribe({
      next: (data) => { this.result.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  search(): void { this.filter = { ...this.filter, page: 1, search: this.searchTerm || undefined }; this.load(); }
  onPageChange(page: number): void { this.filter = { ...this.filter, page }; this.load(); }
  confirmDelete(course: Course): void { this.deleteTarget.set(course); }
  cancelDelete(): void { this.deleteTarget.set(null); }

  doDelete(): void {
    const c = this.deleteTarget();
    if (!c) return;
    this.courseService.delete(c.id).subscribe(() => { this.deleteTarget.set(null); this.load(); });
  }

  toggleActive(course: Course): void {
    (course.isActive ? this.courseService.deactivate(course.id) : this.courseService.activate(course.id)).subscribe(() => this.load());
  }
}
