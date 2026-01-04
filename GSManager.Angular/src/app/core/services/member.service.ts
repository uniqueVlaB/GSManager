import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { MemberDto, MemberListItem, MemberQueryParams, CreateMemberDto } from '../../shared/models';
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

  private readonly membersSignal = signal<MemberDto[]>([]);
  private readonly loadingSignal = signal(false);
  private readonly successSignal = signal<string | null>(null);

  readonly members = this.membersSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();
  readonly success = this.successSignal.asReadonly();

  readonly membersList = computed<MemberListItem[]>(() =>
    this.membersSignal()
      .filter((m): m is MemberDto & { id: string } => m.id !== null)
      .map(m => ({
        id: m.id,
        firstName: m.firstName ?? '',
        lastName: m.lastName ?? '',
        email: m.email ?? '',
        phoneNumber: m.phoneNumber,
        plotIds: m.plotIds
      }))
  );

  readonly totalCount = computed(() => this.membersSignal().length);

  /**
   * Load all members with optional query filters
   */
  async loadMembers(params?: MemberQueryParams): Promise<void> {
    this.loadingSignal.set(true);

    const httpParams = HttpUtils.createParams(params);

    try {
      const members = await firstValueFrom(this.http.get<MemberDto[]>(this.apiUrl, { params: httpParams }));
      this.membersSignal.set(members);
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
  async getMemberById(id: string): Promise<void> {
    this.loadingSignal.set(true);
    try {
      const member = await firstValueFrom(this.http.get<MemberDto>(`${this.apiUrl}/${id}`));
      this.membersSignal.update(members => {
        const index = members.findIndex(m => m.id === id);
        return index >= 0
          ? members.map(m => m.id === id ? member : m)
          : [...members, member];
      });
    }
    catch (err) {
      console.error('Failed to get member:', err);
      this.toastService.error('Failed to get member');
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
    this.successSignal.set(null);

    try {
      const newMember = await firstValueFrom(this.http.post<MemberDto>(this.apiUrl, member));
      
      this.membersSignal.update(members => [...members, newMember]);
      const successMsg = `Member ${newMember.firstName} ${newMember.lastName} added successfully`;
      this.toastService.success(successMsg);
      this.successSignal.set(successMsg);
      
      // Clear success message after 3 seconds
      setTimeout(() => this.successSignal.set(null), 3000);
    } catch (err: any) {
      console.error('Failed to add member:', err);
      const errorMsg = err.error?.detail || 'Failed to add member';
      this.toastService.error(errorMsg);
    } finally {
      this.loadingSignal.set(false);
    }
  }

  /**
   * Clear success state
   */
  clearSuccess(): void {
    this.successSignal.set(null);
  }
}
