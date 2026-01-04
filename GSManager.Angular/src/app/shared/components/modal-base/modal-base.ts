import { Component, ChangeDetectionStrategy, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-modal-base',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal-base.html',
  styleUrl: './modal-base.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModalBaseComponent {
  readonly title = input.required<string>();
  readonly close = output<void>();

  onBackdropClick(): void {
    this.close.emit();
  }

  closeModal(): void {
    this.close.emit();
  }
}
