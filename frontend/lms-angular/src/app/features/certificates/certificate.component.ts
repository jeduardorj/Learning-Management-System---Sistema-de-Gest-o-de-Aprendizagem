import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CertificateService } from '../../core/services/certificate.service';
import { LoadingComponent } from '../../shared/components/loading/loading.component';

@Component({
  selector: 'app-certificate',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingComponent],
  templateUrl: './certificate.component.html'
})
export class CertificateComponent implements OnInit {
  certificate = signal<any>(null);
  loading = signal(true);
  notFound = signal(false);

  constructor(private route: ActivatedRoute, private certificateService: CertificateService) {}

  ngOnInit(): void {
    const courseId = this.route.snapshot.params['courseId'];
    this.certificateService.getMyCertificate(courseId).subscribe({
      next: (c) => { this.certificate.set(c); this.loading.set(false); },
      error: () => { this.notFound.set(true); this.loading.set(false); }
    });
  }
}
