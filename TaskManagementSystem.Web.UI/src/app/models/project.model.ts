export interface ProjectResponseDto{
    ProjectId:number;
    ProjectKey: string | null;
    name? :string | null;
    description? :string | null;
    leadUserId:number;
    createdBy?: number | null;
    createdOn?: string | null;
    modifiedBy?: number | null;
    modifiedOn?: string | null;
    isActive?: boolean | null;
    createdDate: string;
}

export interface CreatePortfolioDto{
    name: string;
    description: string | null;
    ownerId:number;
    isActive:boolean;
}

export interface UpdatePortfolioDto{
    name: string;
    description: string | null;
    ownerId:number;
    isActive:boolean;
}