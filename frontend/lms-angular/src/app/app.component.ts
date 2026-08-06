import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { AuthService } from './core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, CommonModule],
  template: `
    @if (authService.isAuthenticated()) {
      <app-navbar />
    }
    <main class="max-w-7xl mx-auto px-4 py-6">
      <router-outlet />
    </main>
  `
})
export class AppComponent {
  constructor(public authService: AuthService) {}
}
