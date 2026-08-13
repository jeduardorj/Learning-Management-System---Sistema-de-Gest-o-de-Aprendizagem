import { Component } from '@angular/core';

@Component({
  selector: 'app-loading',
  standalone: true,
  template: `<div class="flex justify-center items-center py-12"><div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-600"></div></div>`
})
export class LoadingComponent {}
