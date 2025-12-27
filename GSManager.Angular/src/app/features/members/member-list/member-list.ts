import { Component, ChangeDetectionStrategy, inject, signal, computed } from '@angular/core';
import { MemberService } from '../../../core/services';
import { MembershipStatus } from '../../../shared/models';
import { ButtonComponent } from '../../../shared/components';

@Component({
  selector: 'app-member-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="page-container">
      <header class="page-header">
        <div class="page-title-section">
          <h1 class="page-title">Members</h1>
          <p class="page-subtitle">Manage your garden society members</p>
        </div>
        <app-button variant="primary" size="md" icon="+" (click)="addMember()">
          Add Member
        </app-button>

      </header>

      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-icon stat-icon-total">👥</div>
          <div class="stat-content">
            <span class="stat-value">{{ memberService.totalCount() }}</span>
            <span class="stat-label">Total Members</span>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon stat-icon-active">✓</div>
          <div class="stat-content">
            <span class="stat-value">{{ memberService.activeCount() }}</span>
            <span class="stat-label">Active</span>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon stat-icon-pending">⏳</div>
          <div class="stat-content">
            <span class="stat-value">{{ pendingCount() }}</span>
            <span class="stat-label">Pending</span>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon stat-icon-inactive">✗</div>
          <div class="stat-content">
            <span class="stat-value">{{ inactiveCount() }}</span>
            <span class="stat-label">Inactive</span>
          </div>
        </div>
      </div>

      <div class="table-container">
        <div class="table-header">
          <div class="search-box">
            <label for="search" class="visually-hidden">Search members</label>
            <input
              id="search"
              type="search"
              class="search-input"
              placeholder="Search members..."
              [value]="searchQuery()"
              (input)="onSearch($event)"
            />
          </div>
          <div class="filter-group">
            <label for="status-filter" class="visually-hidden">Filter by status</label>
            <select 
              id="status-filter" 
              class="filter-select"
              (change)="onStatusFilter($event)">
              <option value="">All statuses</option>
              <option value="active">Active</option>
              <option value="pending">Pending</option>
              <option value="inactive">Inactive</option>
              <option value="suspended">Suspended</option>
            </select>
          </div>
        </div>

        <div class="table-wrapper" role="region" aria-label="Members table" tabindex="0">
          <table class="data-table">
            <thead>
              <tr>
                <th scope="col">Name</th>
                <th scope="col">Email</th>
                <th scope="col">Plot</th>
                <th scope="col">Status</th>
                <th scope="col">
                  <span class="visually-hidden">Actions</span>
                </th>
              </tr>
            </thead>
            <tbody>
              @for (member of filteredMembers(); track member.id) {
                <tr>
                  <td>
                    <div class="member-name">
                      <span class="member-avatar">
                        {{ member.firstName.charAt(0) }}{{ member.lastName.charAt(0) }}
                      </span>
                      <span>{{ member.firstName }} {{ member.lastName }}</span>
                    </div>
                  </td>
                  <td>{{ member.email }}</td>
                  <td>
                    @if (member.plotNumber) {
                      <span class="plot-badge">{{ member.plotNumber }}</span>
                    } @else {
                      <span class="text-muted">—</span>
                    }
                  </td>
                  <td>
                    <span class="status-badge" [class]="'status-' + member.membershipStatus">
                      {{ member.membershipStatus }}
                    </span>
                  </td>
                  <td>
                    <div class="action-buttons">
                      <button 
                        class="btn-icon-action" 
                        aria-label="View {{ member.firstName }} {{ member.lastName }}"
                        title="View details">
                        👁️
                      </button>
                      <button 
                        class="btn-icon-action" 
                        aria-label="Edit {{ member.firstName }} {{ member.lastName }}"
                        title="Edit member">
                        ✏️
                      </button>
                      <button 
                        class="btn-icon-action btn-danger" 
                        aria-label="Delete {{ member.firstName }} {{ member.lastName }}"
                        title="Delete member"
                        (click)="deleteMember(member.id)">
                        🗑️
                      </button>
                    </div>
                  </td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="5" class="empty-state">
                    <div class="empty-content">
                      <span class="empty-icon">👥</span>
                      <p>No members found</p>
                      @if (searchQuery() || statusFilter()) {
                        <button class="btn btn-secondary" (click)="clearFilters()">
                          Clear filters
                        </button>
                      }
                    </div>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .page-container {
      max-width: 1400px;
      margin: 0 auto;
    }

    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 1.5rem;
      gap: 1rem;
      flex-wrap: wrap;
    }

    .page-title {
      font-size: 1.75rem;
      font-weight: 700;
      color: var(--text-color);
      margin: 0;
    }

    .page-subtitle {
      color: var(--text-muted);
      margin: 0.25rem 0 0;
      font-size: 0.9375rem;
    }

    .btn {
      display: inline-flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.625rem 1.25rem;
      font-size: 0.9375rem;
      font-weight: 500;
      border-radius: 8px;
      border: none;
      cursor: pointer;
      transition: all 0.2s;

      &:focus-visible {
        outline: 2px solid var(--primary-color);
        outline-offset: 2px;
      }
    }

    .btn-primary {
      background: var(--primary-color);
      color: var(--text-white);

      &:hover {
        background: var(--primary-hover);
      }
    }

    .btn-secondary {
      background: var(--hover-bg);
      color: var(--text-color);

      &:hover {
        background: var(--border-color);
      }
    }

    .btn-icon {
      font-size: 1.25rem;
      line-height: 1;
    }

    .stats-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 1rem;
      margin-bottom: 1.5rem;
    }

    .stat-card {
      display: flex;
      align-items: center;
      gap: 1rem;
      padding: 1.25rem;
      background: var(--card-bg);
      border-radius: 12px;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      transition: background-color 0.3s;
    }

    .stat-icon {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 48px;
      height: 48px;
      border-radius: 12px;
      font-size: 1.5rem;
      transition: background-color 0.3s;
    }

    .stat-icon-total { background: var(--stat-total-bg); }
    .stat-icon-active { background: var(--stat-active-bg); }
    .stat-icon-pending { background: var(--stat-pending-bg); }
    .stat-icon-inactive { background: var(--stat-inactive-bg); }

    .stat-content {
      display: flex;
      flex-direction: column;
    }

    .stat-value {
      font-size: 1.5rem;
      font-weight: 700;
      color: var(--text-color);
    }

    .stat-label {
      font-size: 0.875rem;
      color: var(--text-muted);
    }

    .table-container {
      background: var(--card-bg);
      border-radius: 12px;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      overflow: hidden;
      transition: background-color 0.3s;
    }

    .table-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1rem 1.25rem;
      border-bottom: 1px solid var(--border-color);
      gap: 1rem;
      flex-wrap: wrap;
    }

    .search-box {
      flex: 1;
      min-width: 200px;
      max-width: 400px;
    }

    .search-input {
      width: 100%;
      padding: 0.625rem 1rem;
      border: 1px solid var(--input-border);
      border-radius: 8px;
      font-size: 0.9375rem;
      background: var(--input-bg);
      color: var(--text-color);
      transition: border-color 0.2s, box-shadow 0.2s, background-color 0.3s;

      &:focus {
        outline: none;
        border-color: var(--primary-color);
        box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.1);
      }

      &::placeholder {
        color: var(--text-muted);
      }
    }

    .filter-select {
      padding: 0.625rem 2rem 0.625rem 1rem;
      border: 1px solid var(--input-border);
      border-radius: 8px;
      font-size: 0.9375rem;
      background: var(--input-bg);
      color: var(--text-color);
      cursor: pointer;
      appearance: none;
      background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' viewBox='0 0 24 24' fill='none' stroke='%236b7280' stroke-width='2'%3E%3Cpath d='M6 9l6 6 6-6'/%3E%3C/svg%3E");
      background-repeat: no-repeat;
      background-position: right 0.75rem center;
      transition: border-color 0.2s, box-shadow 0.2s, background-color 0.3s;

      &:focus {
        outline: none;
        border-color: var(--primary-color);
        box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.1);
      }
    }

    .table-wrapper {
      overflow-x: auto;

      &:focus {
        outline: 2px solid var(--primary-color);
        outline-offset: -2px;
      }
    }

    .data-table {
      width: 100%;
      border-collapse: collapse;
      font-size: 0.9375rem;
    }

    .data-table th {
      text-align: left;
      padding: 0.875rem 1.25rem;
      background: var(--table-header-bg);
      color: var(--text-muted);
      font-weight: 600;
      font-size: 0.8125rem;
      text-transform: uppercase;
      letter-spacing: 0.05em;
      border-bottom: 1px solid var(--border-color);
      transition: background-color 0.3s;
    }

    .data-table td {
      padding: 1rem 1.25rem;
      border-bottom: 1px solid var(--border-color);
      color: var(--text-color);
    }

    .data-table tbody tr {
      transition: background-color 0.15s;

      &:hover {
        background: var(--hover-bg);
      }

      &:last-child td {
        border-bottom: none;
      }
    }

    .member-name {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      font-weight: 500;
      color: var(--text-color);
    }

    .member-avatar {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 36px;
      height: 36px;
      background: var(--avatar-bg);
      color: var(--avatar-color);
      font-weight: 600;
      font-size: 0.8125rem;
      border-radius: 50%;
      transition: background-color 0.3s, color 0.3s;
    }

    .plot-badge {
      display: inline-block;
      padding: 0.25rem 0.625rem;
      background: var(--badge-bg);
      color: var(--text-color);
      font-size: 0.8125rem;
      font-weight: 500;
      border-radius: 6px;
      transition: background-color 0.3s;
    }

    .status-badge {
      display: inline-block;
      padding: 0.25rem 0.75rem;
      font-size: 0.8125rem;
      font-weight: 500;
      border-radius: 9999px;
      text-transform: capitalize;
      transition: background-color 0.3s, color 0.3s;
    }

    .status-active {
      background: var(--status-active-bg);
      color: var(--status-active-color);
    }

    .status-pending {
      background: var(--status-pending-bg);
      color: var(--status-pending-color);
    }

    .status-inactive {
      background: var(--status-inactive-bg);
      color: var(--status-inactive-color);
    }

    .status-suspended {
      background: var(--status-suspended-bg);
      color: var(--status-suspended-color);
    }

    .text-muted {
      color: var(--text-muted);
    }

    .action-buttons {
      display: flex;
      gap: 0.25rem;
    }

    .btn-icon-action {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 32px;
      height: 32px;
      border: none;
      background: transparent;
      border-radius: 6px;
      cursor: pointer;
      font-size: 1rem;
      transition: background-color 0.15s;

      &:hover {
        background: var(--hover-bg);
      }

      &:focus-visible {
        outline: 2px solid var(--primary-color);
        outline-offset: 2px;
      }

      &.btn-danger:hover {
        background: var(--status-suspended-bg);
      }
    }

    .empty-state {
      padding: 3rem !important;
      text-align: center;
    }

    .empty-content {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 0.75rem;
      color: var(--text-muted);
    }

    .empty-icon {
      font-size: 3rem;
      opacity: 0.5;
    }

    .visually-hidden {
      position: absolute;
      width: 1px;
      height: 1px;
      padding: 0;
      margin: -1px;
      overflow: hidden;
      clip: rect(0, 0, 0, 0);
      white-space: nowrap;
      border: 0;
    }

    @media (max-width: 768px) {
      .page-header {
        flex-direction: column;
        align-items: stretch;
      }

      .table-header {
        flex-direction: column;
        align-items: stretch;
      }

      .search-box {
        max-width: none;
      }

      .filter-select {
        width: 100%;
      }
    }
  `],
  imports: [ButtonComponent]
})
export class MemberListComponent {
  protected memberService = inject(MemberService);
  
  searchQuery = signal('');
  statusFilter = signal<MembershipStatus | ''>('');

  filteredMembers = computed(() => {
    let members = this.memberService.membersList();
    
    const query = this.searchQuery().toLowerCase();
    if (query) {
      members = members.filter(m =>
        m.firstName.toLowerCase().includes(query) ||
        m.lastName.toLowerCase().includes(query) ||
        m.email.toLowerCase().includes(query) ||
        m.plotNumber?.toLowerCase().includes(query)
      );
    }

    const status = this.statusFilter();
    if (status) {
      members = members.filter(m => m.membershipStatus === status);
    }

    return members;
  });

  pendingCount = computed(() =>
    this.memberService.members().filter(m => m.membershipStatus === 'pending').length
  );

  inactiveCount = computed(() =>
    this.memberService.members().filter(m => m.membershipStatus === 'inactive').length
  );

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchQuery.set(input.value);
  }

  onStatusFilter(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.statusFilter.set(select.value as MembershipStatus | '');
  }

  clearFilters(): void {
    this.searchQuery.set('');
    this.statusFilter.set('');
  }

  addMember(): void {
    // TODO: Navigate to add member form or open modal
    console.log('Add member clicked');
  }

  deleteMember(id: string): void {
    if (confirm('Are you sure you want to delete this member?')) {
      this.memberService.deleteMember(id);
    }
  }
}
