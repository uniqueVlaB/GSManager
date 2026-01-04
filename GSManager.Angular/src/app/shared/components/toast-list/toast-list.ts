import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../core/services';

@Component({
  selector: 'app-toast-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast-list.html',
  styleUrl: './toast-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToastListComponent {
  private readonly toastService = inject(ToastService);
  readonly toasts = this.toastService.toasts;

  remove(id: string): void {
    this.toastService.remove(id);
  }
}
