import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (visible) {
      <div class="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
        <div class="bg-white rounded-lg shadow-xl p-6 max-w-sm w-full mx-4">
          <h3 class="text-lg font-semibold text-gray-800 mb-2">{{ title }}</h3>
          <p class="text-gray-600 mb-6">{{ message }}</p>
          <div class="flex gap-3 justify-end">
            <button (click)="cancel.emit()" class="px-4 py-2 text-sm border rounded hover:bg-gray-50">Cancelar</button>
            <button (click)="confirm.emit()" class="px-4 py-2 text-sm bg-red-600 text-white rounded hover:bg-red-700">Confirmar</button>
          </div>
        </div>
      </div>
    }
  `
})
export class ConfirmDialogComponent {
  @Input() visible = false;
  @Input() title = 'Confirmar';
  @Input() message = 'Tem certeza?';
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
}
