export interface ICreateScheduledPlanRequest {
    planId: string,

    type: string,

    cronExpressionUtc: string | null,
    executeUtc: string | null,

    arguments: string | null
}