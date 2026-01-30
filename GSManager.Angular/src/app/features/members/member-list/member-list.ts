import { Component, ChangeDetectionStrategy, inject, signal, computed, OnInit, input} from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { MemberService, PlotService, PriviledgeService } from '../../../core/services';
import { ButtonComponent, PageSelectorComponent } from '../../../shared/components';
import { UpsertMemberModalComponent, MemberDetailsModalComponent } from '../modals';
import { FullMemberDto, MemberDto } from '../../../shared/models';
import { RoleService } from '../../../core/services/role.service';

@Component({
  selector: 'app-member-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './member-list.html',
  styleUrl: './member-list.scss',
  imports: [ButtonComponent, UpsertMemberModalComponent,MemberDetailsModalComponent, PageSelectorComponent]
})
export class MemberListComponent implements OnInit {
  readonly id = input<string>();

  protected readonly memberService = inject(MemberService);
  protected readonly plotService = inject(PlotService);
  protected readonly priviledgeService = inject(PriviledgeService);
  protected readonly roleService = inject(RoleService);

  
  readonly loading = computed(() => this.memberService.loading()|| this.plotService.loading() || this.priviledgeService.loading() || this.roleService.loading());
  
  readonly fullMembers = computed<FullMemberDto[]>(() => {
    const members = this.memberService.members();
    const plots = this.plotService.plots();
    const priviledges = this.priviledgeService.priveleges();
    const roles = this.roleService.roles();

    return members.map(member => {
      const memberPlots = plots.filter(p => p.ownerId === member.id);
      const memberPriviledge = member.priviledgeId
        ? priviledges.find(p => p.id === member.priviledgeId) ?? undefined
        : undefined;
      const memberRole = member.roleId
        ? roles.find(r => r.id === member.roleId) ?? undefined
        : undefined;

      return {
        ...member,
        plots: memberPlots,
        priviledge: memberPriviledge,
        role: memberRole
      };
    });
  });

  readonly searchQuery = signal('');
  readonly showUpsertModal = signal(false);
  readonly showDetailsModal = signal(false);
  readonly selectedMemberId = signal<string | null>(null);
  page = 1;
  pageSize = 10;

  readonly hasActiveFilters = computed(() => Boolean(this.searchQuery()));

  readonly selectedFullMember = computed<FullMemberDto | null>(() => {
    const memberId = this.selectedMemberId();
    if (!memberId) return null;
    return this.fullMembers().find(m => m.id === memberId) ?? null;
  });

  readonly selectedMember = computed<MemberDto | null>(() => {
    const memberId = this.selectedMemberId();
    if (!memberId) return null;

    return this.memberService.members().find(m => m.id === memberId) ?? null;
  });

  constructor() {
    toObservable(this.searchQuery)
      .pipe(
        skip(1),
        debounceTime(600),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.page = 1;
        this.loadData();
      });
  }

  async ngOnInit(): Promise<void> {
    this.loadData();
    if (this.id()) {
      if(await this.memberService.getMemberById(this.id()!)) {
        this.viewLoadedMember(this.id()!);
      }
    }
  }

  private async loadData(): Promise<void> {
    await this.memberService.getMembers({
      searchQuery: this.searchQuery(),
      page: this.page,
      pageSize: this.pageSize
    });

    let tasks = [];
    const currentMembers = this.memberService.members();

    const ownerIds = [...new Set(currentMembers
      .map(m => m.id)
      .filter((id): id is string => id != null))];
    if (ownerIds.length > 0) {
      tasks.push(this.plotService.getPlots({ ownerIds }));
    }

    const priviledgeIds = [...new Set(currentMembers
      .map(m => m.priviledgeId)
      .filter((id): id is string => id != null))];
    if (priviledgeIds.length > 0) {
      tasks.push(this.priviledgeService.getPriviledges({ ids: priviledgeIds }));
    }
    
    const roleIds = [...new Set(currentMembers
      .map(m => m.roleId)
      .filter((id): id is string => id != null))];
    if (roleIds.length > 0) {
      tasks.push(this.roleService.getRoles({ ids: roleIds }));
    }

    await Promise.all(tasks);
    document.getElementById('main-content')?.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchQuery.set(input.value);
  }

  clearFilters(): void {
    this.searchQuery.set('');
  }

  showUpsertMemberModal(): void {
    this.showUpsertModal.set(true);
  }

  onUpsertModalClose(): void {
    this.showUpsertModal.set(false);
    this.selectedMemberId.set(null);
  }

  async onUpsertModalSubmit(memberData: MemberDto): Promise<void> {
    if (this.selectedMemberId()) {
      await this.memberService.updateMember(memberData);
      this.selectedMemberId.set(null);
    } else {
      await this.memberService.addMember(memberData);
    }
    this.showUpsertModal.set(false);
    this.selectedMemberId.set(null);
    this.loadData();
  }

  viewMember(id: string): void {
    this.selectedMemberId.set(id);
    this.memberService.getMemberById(id);
    this.showDetailsModal.set(true);
  }

  viewLoadedMember(id: string): void {
    this.selectedMemberId.set(id);
    this.showDetailsModal.set(true);
  }

  onDetailsModalClose(): void {
    this.showDetailsModal.set(false);
    this.selectedMemberId.set(null);
  }

  onDetailsModalEdit(id: string): void {
    this.selectedMemberId.set(id);
    this.showDetailsModal.set(false);
    this.showUpsertModal.set(true);
  }

  async setPage(page: number): Promise<void> {
    this.page = page;
    await this.loadData();
  }

  async setPageSize(size: number | string): Promise<void> {
    this.pageSize = Number(size);
    this.page = 1;
    await this.loadData();
  }
}