export interface IScheduledPlan{
    id: string,
    
    type: string,
    hangfireId: string,
    executeUtc: string,
    cronExpressionUct: string,

    planId: string,
}