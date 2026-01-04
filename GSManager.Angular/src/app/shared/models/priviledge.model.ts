

export interface PriviledgeDto {
  id: string;
  name: string;
  description?: string;
}

export interface CreatePriviledgeDto {
  name: string;
  description?: string;
}

export interface PriviledgeQueryParams {
  name?: string;
  ids?: string[];
}