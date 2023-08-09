import { IScheduledPlan } from "./scheduledPlan";

export interface IPlan{
    id: string,
    
    name: string,
    information: string,
    executionPath: string,
    type: string,

    scheduledPlans: IScheduledPlan
}