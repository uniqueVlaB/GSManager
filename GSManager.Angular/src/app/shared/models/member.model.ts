import { PagedRequest } from "./paged-request.model";
import { PlotDto } from "./plot.model";
import { PriviledgeDto } from "./priviledge.model";
import { RoleDto } from "./role.model";

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

export interface FullMemberDto extends Omit<MemberDto, 'roleId' | 'priviledgeId' | 'plotIds'> {
  role?: RoleDto;
  priviledge?: PriviledgeDto;
  plots?: PlotDto[];
}

/**
 * Query parameters for filtering members
 */
export interface MemberQueryParams extends PagedRequest {
  firstName?: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  ids?: string[];
  roleId?: string;
  priviledgeId?: string;
  plotId?: string;
  searchQuery?: string;
}

/**
 * Member for creating/updating (without id)
 */
export type CreateMemberDto = Omit<MemberDto, 'id'>;

