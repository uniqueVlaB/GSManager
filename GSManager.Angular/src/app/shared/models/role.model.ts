export interface RoleDto {
  id: string;
  name: string;
  description?: string;
  priviledgeId?: string;
}

export interface RoleQueryParams {
  name?: string;
  ids?: string[];
  searchQuery?: string;
  priviledgeIds?: string[];
}