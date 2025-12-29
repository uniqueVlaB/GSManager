import { Component, ChangeDetectionStrategy, inject, signal, computed, OnInit } from '@angular/core';
import { MemberService } from '../../../core/services';
import { ButtonComponent } from '../../../shared/components';
import { AddMemberModalComponent, MemberDetailsModalComponent } from '../modals';
import { MemberDto } from '../../../shared/models';

@Component({
  selector: 'app-member-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './member-list.html',
  styleUrl: './member-list.scss',
  imports: [ButtonComponent, AddMemberModalComponent, MemberDetailsModalComponent]
})
export class MemberListComponent implements OnInit {
  protected readonly memberService = inject(MemberService);

  // Expose service state
  readonly loading = this.memberService.loading;
  readonly error = this.memberService.error;
  readonly success = this.memberService.success;

  // Local modal state
  readonly searchQuery = signal('');
  readonly showAddModal = signal(false);
  readonly showDetailsModal = signal(false);
  readonly selectedMemberId = signal<string | null>(null);

  // Derived state
  readonly filteredMembers = computed(() => {
    const query = this.searchQuery().toLowerCase();

    return this.memberService.membersList().filter(member => {
      return !query ||
        member.firstName.toLowerCase().includes(query) ||
        member.lastName.toLowerCase().includes(query) ||
        member.email.toLowerCase().includes(query);
    });
  });

  readonly hasActiveFilters = computed(() => Boolean(this.searchQuery()));

  readonly selectedMember = computed<MemberDto | undefined>(() => {
    const memberId = this.selectedMemberId();
    if (!memberId) return undefined;
    return this.memberService.members().find(m => m.id === memberId);
  });

  ngOnInit(): void {
    this.memberService.loadMembers();
  }

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchQuery.set(input.value);
  }

  clearFilters(): void {
    this.searchQuery.set('');
  }

  addMember(): void {
    this.showAddModal.set(true);
  }

  onAddMemberClose(): void {
    this.showAddModal.set(false);
  }

  onAddMemberSubmit(memberData: any): void {
    if (!memberData || !memberData.firstName || !memberData.lastName) {
      console.error('Invalid member data:', memberData);
      return;
    }
    this.memberService.addMember(memberData);
    this.showAddModal.set(false);
  }

  viewMember(id: string): void {
    this.selectedMemberId.set(id);
    this.memberService.getMemberById(id);
    this.showDetailsModal.set(true);
  }

  onDetailsModalClose(): void {
    this.showDetailsModal.set(false);
    this.selectedMemberId.set(null);
  }

  onDetailsModalEdit(id: string): void {
    // TODO: Navigate to edit member form
    console.log('Edit member:', id);
    this.showDetailsModal.set(false);
  }

  editMember(id: string): void {
    // TODO: Navigate to edit member form
    console.log('Edit member:', id);
  }
}
