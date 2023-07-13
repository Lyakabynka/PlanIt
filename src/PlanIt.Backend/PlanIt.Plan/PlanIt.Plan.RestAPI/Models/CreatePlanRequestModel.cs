using System.Text.Json.Serialization;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.RestAPI.Models;

public class CreatePlanRequestModel
{
    public string Name { get; set; }
    public string Information { get; set; }
    public string? ExecutionPath { get; set; }
    
    public PlanType Type { get; set; }
}