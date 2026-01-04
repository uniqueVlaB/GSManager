/**
 * Member DTO matching the API schema
 */
export interface MemberDto {
  id: string | null;
  firstName: string | null;
  middleName: string | null;
  lastName: string | null;
  phoneNumber: string | null;
  email: string | null;
  roleId: string | null;
  priviledgeId: string | null;
  plotIds: string[] | null;
}

/**
 * Query parameters for filtering members
 */
export interface MemberQueryParams {
  firstName?: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  ids?: string[];
  roleId?: string;
  priviledgeId?: string;
  plotId?: string;
}

/**
 * Member for creating/updating (without id)
 */
export type CreateMemberDto = Omit<MemberDto, 'id'>;

/**
 * Simplified member for list display
 */
export interface MemberListItem {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  plotIds: string[] | null;
}
