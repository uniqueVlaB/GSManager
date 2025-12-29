import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MemberDto, MemberListItem, MemberQueryParams, CreateMemberDto } from '../../shared/models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/members`;

  // State signals
  private readonly membersSignal = signal<MemberDto[]>([]);
  private readonly loadingSignal = signal(false);
  private readonly errorSignal = signal<string | null>(null);
  private readonly successSignal = signal<string | null>(null);

  // Public readonly signals
  readonly members = this.membersSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();
  readonly error = this.errorSignal.asReadonly();
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
  loadMembers(params?: MemberQueryParams): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    let httpParams = new HttpParams();
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        if (value) {
          httpParams = httpParams.set(key, value);
        }
      });
    }

    this.http.get<MemberDto[]>(this.apiUrl, { params: httpParams }).subscribe({
      next: (members) => {
        this.membersSignal.set(members);
        this.loadingSignal.set(false);
      },
      error: (err) => {
        console.error('Failed to load members:', err);
        this.errorSignal.set('Failed to load members');
        this.loadingSignal.set(false);
      }
    });
  }

  /**
   * Get a single member by ID
   */
  getMemberById(id: string): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);

    this.http.get<MemberDto>(`${this.apiUrl}/${id}`).subscribe({
      next: (member) => {
        this.membersSignal.update(members => {
          const index = members.findIndex(m => m.id === id);
          return index >= 0
            ? members.map(m => m.id === id ? member : m)
            : [...members, member];
        });
        this.loadingSignal.set(false);
      },
      error: (err) => {
        console.error('Failed to get member:', err);
        this.errorSignal.set('Failed to get member');
        this.loadingSignal.set(false);
      }
    });
  }

  /**
   * Create a new member
   */
  addMember(member: CreateMemberDto): void {
    this.loadingSignal.set(true);
    this.errorSignal.set(null);
    this.successSignal.set(null);

    this.http.post<MemberDto>(this.apiUrl, member).subscribe({
      next: (newMember) => {
        this.membersSignal.update(members => [...members, newMember]);
        this.successSignal.set(`Member ${newMember.firstName} ${newMember.lastName} added successfully`);
        this.loadingSignal.set(false);
        // Clear success message after 3 seconds
        setTimeout(() => this.successSignal.set(null), 3000);
      },
      error: (err) => {
        console.error('Failed to add member:', err);
        const errorMsg = err.error?.detail || 'Failed to add member';
        this.errorSignal.set(errorMsg);
        this.loadingSignal.set(false);
      }
    });
  }

  /**
   * Clear error state
   */
  clearError(): void {
    this.errorSignal.set(null);
  }

  /**
   * Clear success state
   */
  clearSuccess(): void {
    this.successSignal.set(null);
  }
}
