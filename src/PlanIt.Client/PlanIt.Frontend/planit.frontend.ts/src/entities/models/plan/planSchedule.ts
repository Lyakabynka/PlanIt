import { IScheduledPlan } from "../scheduledPlan/scheduledPlan";

export interface IPlanSchedule{
    id: string,
    
    name: string,
    
    scheduledPlans: IScheduledPlan[]
}