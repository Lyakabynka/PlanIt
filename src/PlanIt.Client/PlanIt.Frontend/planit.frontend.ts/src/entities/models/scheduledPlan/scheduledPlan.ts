export interface IScheduledPlan{
    id: string,
    
    type: string,
    hangfireId: string,
    executeUtc: string,
    cronExpressionUtc: string,
}