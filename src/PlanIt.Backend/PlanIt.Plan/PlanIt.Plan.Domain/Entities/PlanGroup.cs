﻿namespace PlanIt.Plan.Domain.Entities;

public class PlanGroup : BaseEntity
{
    public string Name { get; set; }
    public List<PlanPlanGroup> PlanPlanGroups { get; set; }
    
    public Guid UserId { get; set; }
 
    //in the future: public string HexColor { get; set; }
}