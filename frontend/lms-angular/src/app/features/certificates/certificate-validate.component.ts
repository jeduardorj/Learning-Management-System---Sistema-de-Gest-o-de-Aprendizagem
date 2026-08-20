import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { CertificateService } from '../../core/services/certificate.service';
import { LoadingComponent } from '../../shared/components/loading/loading.component';

@Component({
  selector: 'app-certificate-validate',
  standalone: true,
  imports: [CommonModule, LoadingComponent],
  templateUrl: './certificate-validate.component.html'
})
export class CertificateValidateComponent implements OnInit {
  result = signal<any>(null);
  loading = signal(true);

  constructor(private route: ActivatedRoute, private certificateService: CertificateService) {}

  ngOnInit(): void {
    const code = this.route.snapshot.params['code'];
    this.certificateService.validate(code).subscribe({
      next: (r) => { this.result.set(r); this.loading.set(false); },
      error: () => { this.result.set({ isValid: false }); this.loading.set(false); }
    });
  }
}
