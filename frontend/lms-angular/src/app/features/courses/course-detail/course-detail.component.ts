import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-course-detail',
  standalone: true,
  imports: [CommonModule],
  template: `<h1 class="text-2xl font-bold">Detalhe do Curso</h1>`
})
export class CourseDetailComponent {}
