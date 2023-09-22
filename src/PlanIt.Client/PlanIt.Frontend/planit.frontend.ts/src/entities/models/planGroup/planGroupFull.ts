import { IPlanPlanGroup } from "./planPlanGroup";

export interface IPlanGroupFull{
    id: string,
    
    name: string,
    planPlanGroups: IPlanPlanGroup[]
}