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
  number?: string;
}

export interface FullPlotDto extends PlotDto {
  owner?: MemberDto | null;
  priviledge?: PriviledgeDto | null;
}

export interface CreatePlotDto {
  number: string;
  square: number;
  description?: string;
  cadastreNumber?: string;
}
