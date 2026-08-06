import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-courses',
  standalone: true,
  imports: [CommonModule],
  template: `<h1 class="text-2xl font-bold">Cursos</h1>`
})
export class CoursesComponent {}
