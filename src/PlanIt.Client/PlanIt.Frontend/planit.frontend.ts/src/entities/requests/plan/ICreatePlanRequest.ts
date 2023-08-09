export interface ICreatePlanRequest {
    type: string,
    name: string,
    information: string,
    executionPath: string | null
}