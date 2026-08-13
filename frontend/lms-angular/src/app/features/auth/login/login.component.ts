import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  form: FormGroup;
  loading = signal(false);
  error = signal('');

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.form = this.fb.group({ email: ['', [Validators.required, Validators.email]], password: ['', Validators.required] });
  }

  onSubmit(): void {
    if (this.form.invalid || this.loading()) return;
    this.loading.set(true);
    this.error.set('');
    this.authService.login(this.form.value).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err: any) => { this.error.set(err.error?.message || 'E-mail ou senha invalidos.'); this.loading.set(false); }
    });
  }
}
