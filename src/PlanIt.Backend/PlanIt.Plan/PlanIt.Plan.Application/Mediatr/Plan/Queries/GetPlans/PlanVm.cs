using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;

public class PlanVm
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }
}