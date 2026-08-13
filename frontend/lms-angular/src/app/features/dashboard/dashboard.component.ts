import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';
import { environment } from '../../../environments/environment';
import { LoadingComponent } from '../../shared/components/loading/loading.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingComponent],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  data = signal<any>(null);
  loading = signal(true);

  constructor(private http: HttpClient, public authService: AuthService) {}

  ngOnInit(): void {
    const endpoint = this.authService.isAdmin() ? 'admin' : 'student';
    this.http.get(`${environment.apiUrl}/dashboard/${endpoint}`).subscribe({
      next: (d) => { this.data.set(d); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}
