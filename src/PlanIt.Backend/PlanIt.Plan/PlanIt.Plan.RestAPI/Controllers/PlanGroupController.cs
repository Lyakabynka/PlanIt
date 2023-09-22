using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.CreatePlanGroup;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.DeletePlanGroup;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.SetPlansToPlanGroup;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroup;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;
using PlanIt.Plan.RestAPI.Models;

namespace PlanIt.Plan.RestAPI.Controllers;

public class PlanGroupController : ApiControllerBase
{
    /// <summary>
    /// Gets PlanGroups
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /plan-groups
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpGet("plan-groups")]
    [ProducesResponseType(typeof(List<Domain.Entities.PlanGroup>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> GetPlanGroups()
    {
        var request = new GetPlanGroupVmsCommand()
        {
            UserId = UserId
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Create PlanGroup
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plan-group
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpPost("plan-group")]
    [ProducesResponseType(typeof(PlanGroupFullVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> CreatePlanGroup([FromBody] CreatePlanGroupRequestModel requestModel)
    {
        var request = new CreatePlanGroupCommand()
        {
            Name = requestModel.Name.Trim(),
            UserId = UserId
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Set plans to plangroup
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plan-group/048fbe34-410c-4ff8-a641-2077a2f09a54/plans
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpPost("plan-group/{id:guid}/plans")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> SetPlansToPlanGroup(
        [FromRoute] Guid id,
        [FromBody] List<SetPlanToPlanGroupRequestModel> requestModels)
    {
        var request = new SetPlansToPlanGroupCommand()
        {
            SetPlanToPlanGroupRequestModels = requestModels,
            PlanGroupId = id,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Delete PlanGroup
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /plan-group/048fbe34-410c-4ff8-a641-2077a2f09a54
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpDelete("plan-group/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> DeletePlanGroup([FromRoute] Guid id)
    {
        var request = new DeletePlanGroupCommand()
        {
            PlanGroupId = id,
            UserId = UserId,
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Gets Plans in specific PlanGroup
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /plan-group/048fbe34-410c-4ff8-a641-2077a2f09a54
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is forbidden</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpGet("plan-group/{id:guid}")]
    [ProducesResponseType(typeof(List<PlanPlanGroupVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> GetPlanGroup([FromRoute] Guid id)
    {
        var request = new GetPlanGroupCommand()
        {
            PlanGroupId = id,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }
}