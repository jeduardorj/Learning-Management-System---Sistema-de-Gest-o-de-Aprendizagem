import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="flex items-center justify-between mt-6">
      <span class="text-sm text-gray-600">{{ totalItems }} resultado(s) - Pagina {{ currentPage }} de {{ totalPages }}</span>
      <div class="flex gap-2">
        <button (click)="pageChange.emit(currentPage - 1)" [disabled]="!hasPreviousPage" class="px-3 py-1 rounded border text-sm disabled:opacity-40 hover:bg-gray-100">Anterior</button>
        <button (click)="pageChange.emit(currentPage + 1)" [disabled]="!hasNextPage" class="px-3 py-1 rounded border text-sm disabled:opacity-40 hover:bg-gray-100">Proximo</button>
      </div>
    </div>
  `
})
export class PaginationComponent {
  @Input() currentPage = 1;
  @Input() totalPages = 1;
  @Input() totalItems = 0;
  @Input() hasPreviousPage = false;
  @Input() hasNextPage = false;
  @Output() pageChange = new EventEmitter<number>();
}
