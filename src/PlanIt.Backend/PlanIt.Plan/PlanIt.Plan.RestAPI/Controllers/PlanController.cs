using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Mediator.Plan.Commands;
using PlanIt.Plan.Application.Mediator.Plan.Queries;
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
    /// <param name="requestModel"/>
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

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(success.Value));
    }

    /// <summary>
    /// Updates the Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// PUT /plan
    /// </remarks>
    /// <param name="requestModel"/>
    /// <returns>Returns Plan</returns>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpPut("plan")]
    [ProducesResponseType(typeof(Domain.Entities.Plan),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePlan(UpdatePlanRequestModel requestModel)
    {
        var request = new UpdatePlanCommand()
        {
            PlanId = requestModel.PlanId,
            Name = requestModel.Name,
            Information = requestModel.Information,
            ExecutionPath = requestModel.ExecutionPath,
            Type = requestModel.Type,
            UserId = UserId,
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(success.Value),
            notFound => NotFound(),
            forbidden => Forbid());
    }

    /// <summary>
    /// Deletes the Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /plan/048fbe34-410c-4ff8-a641-2077a2f09a54
    /// </remarks>
    /// <param name="id"/>
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

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid());
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
    [ProducesResponseType(typeof(List<Domain.Entities.Plan>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlans()
    {
        var request = new GetPlansQuery()
        {
            UserId = UserId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(success.Value),
            notFound => NotFound());
    }

    /// <summary>
    /// Fires Instant Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plans/048fbe34-410c-4ff8-a641-2077a2f09a54/instant
    /// </remarks>
    /// <param name="id"/>
    /// <response code="200">Success</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpPost("plan/{id:guid}/instant")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScheduleInstantPlan([FromRoute] Guid id)
    {
        var request = new ScheduleInstantPlanCommand()
        {
            PlanId = id,
            UserId = UserId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid());
    }

    /// <summary>
    /// Schedules OneOff Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plans/048fbe34-410c-4ff8-a641-2077a2f09a54/one-off
    /// </remarks>
    /// <param name="id"/>
    /// <param name="requestModel"/>
    /// <response code="200">Success</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpPost("plan/{id:guid}/one-off")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScheduleOneOffPlan([FromRoute] Guid id,
        [FromBody] ScheduleOneOffPlanRequestModel requestModel)
    {
        var request = new ScheduleOneOffPlanCommand()
        {
            PlanId = id,
            ExecuteUtc = requestModel.ExecuteUtc,
            UserId = UserId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid(),
            badRequest => BadRequest(badRequest.Error));
    }

    /// <summary>
    /// Schedules Recurring Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plans/048fbe34-410c-4ff8-a641-2077a2f09a54/recurring
    /// </remarks>
    /// <param name="id"/>
    /// <param name="requestModel"/>
    /// <response code="200">Success</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpPost("plan/{id:guid}/recurring")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ScheduleRecurringPlan([FromRoute] Guid id,
        [FromBody] ScheduleRecurringPlanRequestModel requestModel)
    {
        var request = new ScheduleRecurringPlanCommand()
        {
            PlanId = id,
            CronExpressionUtc = requestModel.CronExpressionUtc,
            UserId = UserId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid(),
            badRequest => BadRequest(badRequest.Error));
    }

    /// <summary>
    /// Deletes OneOff Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /plans/048fbe34-410c-4ff8-a641-2077a2f09a54/one-off
    /// </remarks>
    /// <param name="id"/>
    /// <response code="200">Success</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpDelete("plan/{id:guid}/one-off")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteOneOffPlan([FromRoute] Guid id)
    {
        var request = new DeleteOneOffPlanCommand()
        {
            OneOffPlanId = id,
            UserId = UserId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid());
    }
    
    /// <summary>
    /// Deletes Recurring Plan
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /plans/048fbe34-410c-4ff8-a641-2077a2f09a54/recurring
    /// </remarks>
    /// <param name="id"/>
    /// <response code="200">Success</response>
    /// <response code="404">Plan was not found</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Invalid parameters</response>
    [Authorize]
    [HttpDelete("plan/{id:guid}/recurring")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRecurringPlan([FromRoute] Guid id)
    {
        var request = new DeleteRecurringPlanCommand()
        {
            OneOffPlanId = id,
            UserId = UserId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid());
    }
}