import { Component, ChangeDetectionStrategy, inject, input, output } from '@angular/core';
import { FullMemberDto} from '../../../../shared/models';
import { ModalBaseComponent, ButtonComponent } from '../../../../shared/components';
import { AuthService } from '../../../../core/services';
import { AppPermission } from '../../../../shared/enums/app-permission.enum';

@Component({
  selector: 'app-member-details-modal',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './member-details-modal.html',
  styleUrl: './member-details-modal.scss',
  imports: [ModalBaseComponent, ButtonComponent]
})
export class MemberDetailsModalComponent {
  private readonly authService = inject(AuthService);

  readonly member = input.required<FullMemberDto>();
  readonly close = output<void>();
  readonly edit = output<string>();
  readonly delete = output<string>();

  readonly canEdit = this.authService.hasPermission(AppPermission.EditMembers);
  readonly canDelete = this.authService.hasPermission(AppPermission.DeleteMembers);

  closeModal(): void {
    this.close.emit();
  }

  onEdit(): void {
    const memberId = this.member()?.id;
    if (memberId) {
      this.edit.emit(memberId);
    }
  }

  onDelete(): void {
    const memberId = this.member()?.id;
    if (memberId) {
      this.delete.emit(memberId);
    }
  }
}
