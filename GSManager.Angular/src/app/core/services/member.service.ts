import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { MemberDto, MemberQueryParams, CreateMemberDto, SelectListItem, PaginatedResponse } from '../../shared/models';
import { environment } from '../../../environments/environment';
import { ToastService } from './toast.service';
import { HttpUtils } from '../utils';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private readonly http = inject(HttpClient);
  private readonly toastService = inject(ToastService);
  private readonly apiUrl = `${environment.apiUrl}/members`;

  private readonly paginatedMembersSignal = signal<PaginatedResponse<MemberDto> | null>(null);
  private readonly memberSelectListSignal = signal<SelectListItem[]>([]);
  private readonly loadingSignal = signal(false);


  readonly members = computed(() => this.paginatedMembersSignal()?.items || []);
  readonly memberSelectList = this.memberSelectListSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();
  readonly paginatedMembers = this.paginatedMembersSignal.asReadonly();

  /**
   * Load all members with optional query filters
   */
  async getMembers(params?: MemberQueryParams): Promise<void> {
    this.loadingSignal.set(true);

    const httpParams = HttpUtils.createParams(params);

    try {
      const response = await firstValueFrom(this.http.get<PaginatedResponse<MemberDto>>(this.apiUrl, { params: httpParams }));
      this.paginatedMembersSignal.set(response);
    } catch (err) {
      console.error('Failed to load members:', err);
      this.toastService.error('Failed to load members');
    } finally {
      this.loadingSignal.set(false);
    }
  }

  /**
   * Get a single member by ID
   */
  async getMemberById(id: string): Promise<MemberDto | null> {
    this.loadingSignal.set(true);
    try {
      const member = await firstValueFrom(this.http.get<MemberDto>(`${this.apiUrl}/${id}`));
      this.paginatedMembersSignal.update(paginated => {
        if (!paginated) return paginated;
        const index = paginated.items.findIndex(m => m.id === id);
        return index >= 0
          ? { ...paginated, items: paginated.items.map(m => m.id === id ? member : m) }
          : paginated;
      });
      return member;
    }
    catch (err) {
      console.error('Failed to get member:', err);
      this.toastService.error('Failed to get member');
      return null;
    }
    finally {
      this.loadingSignal.set(false);
    }
  }

  /**
   * Create a new member
   */
  async addMember(member: CreateMemberDto): Promise<void> {
    this.loadingSignal.set(true);

    try {
      const newMember = await firstValueFrom(this.http.post<MemberDto>(this.apiUrl, member));

      this.paginatedMembersSignal.update(paginated => {
        if (!paginated) return paginated;
        return { 
          ...paginated, 
          items: [...paginated.items, newMember],
          totalCount: paginated.totalCount + 1
        };
      });

      const successMsg = `Member ${newMember.firstName} ${newMember.lastName} added successfully`;
      this.toastService.success(successMsg);
    } catch (err: any) {
      console.error('Failed to add member:', err);
      const errorMsg = err.error?.detail || 'Failed to add member';
      this.toastService.error(errorMsg);
    } finally {
      this.loadingSignal.set(false);
    }
  }

  async getSelectList(): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const list = await firstValueFrom(this.http.get<SelectListItem[]>(`${this.apiUrl}/select-list`));
      this.memberSelectListSignal.set(list);
    }
    catch (err) {
      console.error('Failed to get member select list:', err);
      this.toastService.error('Failed to get member select list');
    }
    finally {
      this.loadingSignal.set(false);
    }
  }

  async updateMember(member: MemberDto): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const updatedMember = await firstValueFrom(this.http.put<MemberDto>(`${this.apiUrl}/${member.id}`, member));
      this.paginatedMembersSignal.update(paginated => {
        if (!paginated) return paginated;
        return {
          ...paginated,
          items: paginated.items.map(m => m.id === updatedMember.id ? updatedMember : m)
        };
      });
      this.toastService.success(`Member ${updatedMember.firstName} ${updatedMember.lastName} updated successfully`);
    }
    catch (err) {
      console.error('Failed to update member:', err);
      this.toastService.error('Failed to update member');
    }
    finally {
      this.loadingSignal.set(false);
    }
  }
}
