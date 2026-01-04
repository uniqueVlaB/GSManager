import { Component, ChangeDetectionStrategy, input, output } from '@angular/core';
import { MemberDto } from '../../../../shared/models';
import { ModalBaseComponent, ButtonComponent } from '../../../../shared/components';

@Component({
  selector: 'app-member-details-modal',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './member-details-modal.html',
  styleUrl: './member-details-modal.scss',
  imports: [ModalBaseComponent, ButtonComponent]
})
export class MemberDetailsModalComponent {
  readonly member = input.required<MemberDto>();
  readonly close = output<void>();
  readonly edit = output<string>();

  closeModal(): void {
    this.close.emit();
  }

  onEdit(): void {
    const memberId = this.member()?.id;
    if (memberId) {
      this.edit.emit(memberId);
    }
  }
}
