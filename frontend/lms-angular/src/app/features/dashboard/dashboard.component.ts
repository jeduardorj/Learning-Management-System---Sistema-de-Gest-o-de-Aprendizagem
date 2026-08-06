import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `<h1 class="text-2xl font-bold">Dashboard</h1>`
})
export class DashboardComponent {}
