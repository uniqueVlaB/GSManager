import { Injectable, signal, computed } from '@angular/core';
import { Member, MemberListItem, MembershipStatus } from '../../shared/models';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private membersSignal = signal<Member[]>(this.getMockMembers());
  
  readonly members = this.membersSignal.asReadonly();
  
  readonly membersList = computed<MemberListItem[]>(() => 
    this.membersSignal().map(m => ({
      id: m.id,
      firstName: m.firstName,
      lastName: m.lastName,
      email: m.email,
      plotNumber: m.plotNumber,
      membershipStatus: m.membershipStatus
    }))
  );

  readonly activeCount = computed(() => 
    this.membersSignal().filter(m => m.membershipStatus === 'active').length
  );

  readonly totalCount = computed(() => this.membersSignal().length);

  getMemberById(id: string): Member | undefined {
    return this.membersSignal().find(m => m.id === id);
  }

  addMember(member: Omit<Member, 'id'>): void {
    const newMember: Member = {
      ...member,
      id: crypto.randomUUID()
    };
    this.membersSignal.update(members => [...members, newMember]);
  }

  updateMember(id: string, updates: Partial<Member>): void {
    this.membersSignal.update(members =>
      members.map(m => m.id === id ? { ...m, ...updates } : m)
    );
  }

  deleteMember(id: string): void {
    this.membersSignal.update(members => members.filter(m => m.id !== id));
  }

  private getMockMembers(): Member[] {
    return [
      {
        id: '1',
        firstName: 'Jan',
        lastName: 'Kowalski',
        email: 'jan.kowalski@email.com',
        phone: '+48 123 456 789',
        plotNumber: 'A-12',
        membershipStatus: 'active',
        joinDate: new Date('2020-03-15'),
        address: {
          street: 'ul. Ogrodowa 15',
          city: 'Warszawa',
          postalCode: '00-001',
          country: 'Poland'
        }
      },
      {
        id: '2',
        firstName: 'Anna',
        lastName: 'Nowak',
        email: 'anna.nowak@email.com',
        phone: '+48 987 654 321',
        plotNumber: 'B-05',
        membershipStatus: 'active',
        joinDate: new Date('2019-06-20')
      },
      {
        id: '3',
        firstName: 'Piotr',
        lastName: 'Wiśniewski',
        email: 'piotr.wisniewski@email.com',
        plotNumber: 'C-08',
        membershipStatus: 'pending',
        joinDate: new Date('2024-01-10')
      },
      {
        id: '4',
        firstName: 'Maria',
        lastName: 'Wójcik',
        email: 'maria.wojcik@email.com',
        phone: '+48 555 123 456',
        plotNumber: 'A-03',
        membershipStatus: 'inactive',
        joinDate: new Date('2018-09-01')
      },
      {
        id: '5',
        firstName: 'Tomasz',
        lastName: 'Kamiński',
        email: 'tomasz.kaminski@email.com',
        plotNumber: 'D-22',
        membershipStatus: 'active',
        joinDate: new Date('2021-05-12')
      },
      {
        id: '6',
        firstName: 'Ewa',
        lastName: 'Lewandowska',
        email: 'ewa.lewandowska@email.com',
        phone: '+48 666 789 012',
        plotNumber: 'B-17',
        membershipStatus: 'suspended',
        joinDate: new Date('2022-02-28')
      }
    ];
  }
}
