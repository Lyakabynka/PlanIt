using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Mediatr.Plan.Commands.CreatePlan;
using PlanIt.Plan.Application.Mediatr.Plan.Commands.DeletePlan;
using PlanIt.Plan.Application.Mediatr.Plan.Commands.UpdatePlan;
using PlanIt.Plan.Application.Mediatr.Plan.Queries.GetPlans;
using PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.CreateScheduledPlan;
using PlanIt.Plan.Application.Mediatr.ScheduledPlan.Commands.DeleteScheduledPlan;
using PlanIt.Plan.RestAPI.Models;

namespace PlanIt.Plan.RestAPI.Controllers;

public class PlanController : ApiControllerBase
{
    /// <summary>
    /// Creates a Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plan
    /// </remarks>
    /// <param name="requestModel">CreatePlanRequestModel with necessary fields</param>
    /// <returns>Returns Plan</returns>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpPost("plan")]
    [ProducesResponseType(typeof(Domain.Entities.Plan), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePlan(CreatePlanRequestModel requestModel)
    {
        var request = new CreatePlanCommand()
        {
            Name = requestModel.Name,
            Information = requestModel.Information,
            ExecutionPath = requestModel.ExecutionPath,
            Type = requestModel.Type,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Updates the Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// PUT /plan
    /// </remarks>
    /// <param name="id">Id of the Plan to update</param>
    /// <param name="requestModel">UpdatePlanRequestModel with necessary fields</param>
    /// <returns>Returns Plan</returns>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpPut("plan/{id:guid}")]
    [ProducesResponseType(typeof(Domain.Entities.Plan), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePlan([FromRoute] Guid id, [FromBody] UpdatePlanRequestModel requestModel)
    {
        var request = new UpdatePlanCommand()
        {
            PlanId = id,
            Name = requestModel.Name,
            Information = requestModel.Information,
            ExecutionPath = requestModel.ExecutionPath,
            Type = requestModel.Type,
            UserId = UserId,
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Deletes the Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /plan/048fbe34-410c-4ff8-a641-2077a2f09a54
    /// </remarks>
    /// <param name="id">Id of the Plan to delete</param>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpDelete("plan/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePlan(Guid id)
    {
        var request = new DeletePlanCommand()
        {
            PlanId = id,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Gets Plans
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /plans
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="404">No plans were found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpGet("plans")]
    [ProducesResponseType(typeof(List<Domain.Entities.Plan>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlans()
    {
        var request = new GetPlansQuery()
        {
            UserId = UserId
        };

        return await Mediator.Send(request);
    }

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
    /// <response code="404">Plan was not found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpPost("plan/{id:guid}/schedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SchedulePlan([FromRoute] Guid id,
        [FromBody] SchedulePlanRequestModel requestModel)
    {
        var request = new CreateScheduledPlanCommand()
        {
            PlanId = id,
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
    /// DELETE /plans/scheduled/048fbe34-410c-4ff8-a641-2077a2f09a54
    /// </remarks>
    /// <param name="id">Id of the ScheduledPlan to delete</param>
    /// <response code="200">Success</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpDelete("plan/scheduled/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteOneOffPlan([FromRoute] Guid id)
    {
        var request = new DeleteScheduledPlanCommand()
        {
            ScheduledPlanId = id,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }
}