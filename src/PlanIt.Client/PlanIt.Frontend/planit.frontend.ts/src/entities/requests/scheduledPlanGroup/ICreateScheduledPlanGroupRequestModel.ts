export interface ICreateScheduledPlanGroupRequest {
    planGroupId: string,

    type: string,

    cronExpressionUtc: string | null,
    executeUtc: string | null,

    arguments: string | null
}