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
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
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
    /// <response code="404">Plan was not found</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
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
    /// <response code="404">Plan was not found</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
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
    /// <response code="401">User is not authorized</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
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
}