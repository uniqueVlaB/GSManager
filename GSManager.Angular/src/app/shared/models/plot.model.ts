import { MemberDto } from "./member.model";
import { PriviledgeDto } from "./priviledge.model";

export interface PlotDto {
  id: string;
  number: string;
  square: number | null;
  ownerId: string | null;
  description: string | null;
  cadastreNumber: string | null;
  priviledgeId: string | null;
}

export interface PlotQueryParams {
  numbers?: string[];
  ownerIds?: string[];
  priviledgeIds?: string[];
  cadastreNumbers?: string[];
  searchQuery?: string;
}

export interface FullPlotDto extends Omit<PlotDto, 'ownerId' | 'priviledgeId'> {
  owner?: MemberDto | null;
  priviledge?: PriviledgeDto | null;
}

export interface CreatePlotDto {
  number: string;
  square?: number;
  description?: string;
  cadastreNumber?: string;
  ownerId?: string;
  priviledgeId?: string;
}
