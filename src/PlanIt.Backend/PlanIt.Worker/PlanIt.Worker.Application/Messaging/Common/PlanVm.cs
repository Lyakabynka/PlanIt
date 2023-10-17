using PlanIt.Worker.Domain.Enums;

namespace PlanIt.Messaging;

public class PlanVm
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    public PlanType Type { get; set; }
}