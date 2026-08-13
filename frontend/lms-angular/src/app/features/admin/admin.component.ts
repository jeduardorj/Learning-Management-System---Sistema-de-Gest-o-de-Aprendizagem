import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule],
  template: `<h1 class="text-2xl font-bold">Painel Admin</h1>`
})
export class AdminComponent {}
