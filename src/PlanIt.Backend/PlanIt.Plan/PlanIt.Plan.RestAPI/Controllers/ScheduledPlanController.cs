using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.CreateScheduledPlan;
using PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.DeleteScheduledPlan;
using PlanIt.Plan.Application.Mediatr.ScheduledPlan.Queries.GetScheduledPlans;
using PlanIt.Plan.RestAPI.Models;

namespace PlanIt.Plan.RestAPI.Controllers;

public class ScheduledPlanController : ApiControllerBase
{
    /// <summary>
    /// Schedules created Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plans/scheduled/048fbe34-410c-4ff8-a641-2077a2f09a54
    /// </remarks>
    /// <param name="id">Id of the Plan to schedule</param>
    /// <param name="requestModel">SchedulePlanRequestModel with necessary fields</param>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpPost("scheduled-plan")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateScheduledPlan([FromBody] SchedulePlanRequestModel requestModel)
    {
        var request = new CreateScheduledPlanCommand()
        {
            PlanId = requestModel.PlanId,
            Type = requestModel.Type,
            CronExpressionUtc = requestModel.CronExpressionUtc,
            ExecuteUtc = requestModel.ExecuteUtc,
            Arguments = requestModel.Arguments,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }
    
    /// <summary>
    /// Deletes Scheduled Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /scheduled-plan/048fbe34-410c-4ff8-a641-2077a2f09a54
    /// </remarks>
    /// <param name="id">Id of the ScheduledPlan to delete</param>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpDelete("scheduled-plan/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteScheduledPlan([FromRoute] Guid id)
    {
        var request = new DeleteScheduledPlanCommand()
        {
            ScheduledPlanId = id,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }
    
    /// <summary>
    /// Get scheduled plans
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /scheduled-plans
    /// </remarks>
    /// <param name="planId">Id of the Plan to get its scheduled plans</param>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpGet("scheduled-plans/{planId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetScheduledPlans([FromRoute] Guid planId)
    {
        var request = new GetScheduledPlansQuery()
        {
            PlanId = planId
        };

        return await Mediator.Send(request);
    }
}