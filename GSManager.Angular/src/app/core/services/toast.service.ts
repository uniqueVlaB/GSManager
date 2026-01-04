import { Injectable, signal } from '@angular/core';
import { Toast, ToastType } from '../../shared/models';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  readonly toasts = signal<Toast[]>([]);
  private readonly queue: Toast[] = [];
  private readonly MAX_TOASTS = 5;

  show(message: string, type: ToastType = 'info', duration: number = 3000): void {
    const id = crypto.randomUUID();
    const toast: Toast = { id, message, type, duration };

    const currentToasts = this.toasts();
    if (currentToasts.length < this.MAX_TOASTS) {
      this.addToast(toast);
    } else {
      this.queue.push(toast);
    }
  }

  private addToast(toast: Toast): void {
    this.toasts.update(current => [toast, ...current]);

    if (toast.duration && toast.duration > 0) {
      setTimeout(() => {
        this.remove(toast.id);
      }, toast.duration);
    }
  }

  remove(id: string): void {
    let wasRemoved = false;
    this.toasts.update(current => {
      const filtered = current.filter(t => t.id !== id);
      if (filtered.length < current.length) {
        wasRemoved = true;
      }
      return filtered;
    });
    
    if (wasRemoved && this.queue.length > 0) {
      const nextToast = this.queue.shift();
      if (nextToast) {
        // Add a small delay to make the transition smoother
        setTimeout(() => this.addToast(nextToast), 300);
      }
    }
  }

  success(message: string, duration?: number): void {
    this.show(message, 'success', duration);
  }

  error(message: string, duration?: number): void {
    this.show(message, 'error', duration);
  }

  info(message: string, duration?: number): void {
    this.show(message, 'info', duration);
  }

  warning(message: string, duration?: number): void {
    this.show(message, 'warning', duration);
  }
}
