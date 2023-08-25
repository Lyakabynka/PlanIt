export interface ISchedulePlanRequest {
    type: string,

    cronExpressionUtc: string | null,
    executeUtc: string | null,

    arguments: string | null
}