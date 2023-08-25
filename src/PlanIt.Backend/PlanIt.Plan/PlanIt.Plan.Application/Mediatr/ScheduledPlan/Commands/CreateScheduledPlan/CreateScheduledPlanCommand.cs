using MediatR;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;
using PlanIt.Plan.Domain.Enums;

namespace PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.CreateScheduledPlan;

public class CreateScheduledPlanCommand : IValidatableRequest<Result>
{
    /// <summary>
    /// Id of the plan to schedule
    /// </summary>
    public Guid PlanId { get; set; }

    /// <summary>
    /// <see cref="CronExpressionUtc"/>.
    /// Used, when the <see cref="Type"/> is 'Recurring'.
    /// </summary>
    public string? CronExpressionUtc { get; set; }
    /// <summary>
    /// <see cref="ExecuteUtc"/>.
    /// Used, when the <see cref="Type"/> is 'OneOff'.
    /// </summary>
    public DateTime? ExecuteUtc { get; set; }
    
    /// <summary>
    /// <see cref="Arguments"/>.
    /// Used for firing custom commands i.e. 'volume {number}',
    /// where {number} is any number, said by user.
    /// </summary>
    public string? Arguments { get; set; }
    
    /// <summary>
    /// Id of the user, who tries to schedule
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// <see cref="Type"/>.
    /// Specifies, how plan should be scheduled.
    /// Currently supported: Instant, OneOff, Recurring.
    /// </summary>
    public ScheduledPlanType Type { get; set; }
}