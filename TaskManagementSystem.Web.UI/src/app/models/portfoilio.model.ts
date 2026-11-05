export interface PortfolioResponseDto {
  portfolioId: number;
  name?: string | null;
  code?: string | null;
  description?: string | null;
  ownerId: number;
  createdBy?: number | null;
  createdOn?: string | null;
  modifiedBy?: number | null;
  modifiedOn?: string | null;
  isActive?: boolean | null;
  createdDate: string;
}

export interface CreatePortfolioDto {
  name: string;
  code: string;
  description?: string | null;
  ownerId: number;
  isActive: boolean;
}

export interface UpdatePortfolioDto {
  name: string;
  code: string;
  description?: string | null;
  ownerId: number;
  isActive: boolean;
}