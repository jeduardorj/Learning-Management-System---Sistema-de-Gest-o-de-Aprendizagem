import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EnrollmentService } from '../../core/services/enrollment.service';
import { LoadingComponent } from '../../shared/components/loading/loading.component';

@Component({
  selector: 'app-my-courses',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingComponent],
  templateUrl: './my-courses.component.html'
})
export class MyCoursesComponent implements OnInit {
  enrollments = signal<any[]>([]);
  loading = signal(true);

  constructor(private enrollmentService: EnrollmentService) {}

  ngOnInit(): void {
    this.enrollmentService.getMyEnrollments().subscribe({
      next: (data) => { this.enrollments.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}
