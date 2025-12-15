export interface Member {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  plotNumber?: string;
  membershipStatus: MembershipStatus;
  joinDate: Date;
  address?: Address;
  notes?: string;
}

export interface Address {
  street: string;
  city: string;
  postalCode: string;
  country: string;
}

export type MembershipStatus = 'active' | 'inactive' | 'pending' | 'suspended';

export interface MemberListItem {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  plotNumber?: string;
  membershipStatus: MembershipStatus;
}
