using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.AddPlanToPlanGroup;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Commands.CreatePlanGroup;
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
    [ProducesResponseType(typeof(List<Domain.Entities.Plan>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlanGroups()
    {
        var request = new GetPlanGroupsCommand()
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
    [ProducesResponseType(typeof(List<Domain.Entities.Plan>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePlanGroup([FromBody] CreatePlanGroupRequestModel requestModel)
    {
        var request = new CreatePlanGroupCommand()
        {
            Name = requestModel.Name,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }

    /// <summary>
    /// Add Plan to PlanGroup
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /plan-group/add
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="403">User is not forbidden</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpGet("plan-group/add")]
    [ProducesResponseType(typeof(List<Domain.Entities.Plan>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPlanToPlanGroup([FromBody] AddPlanToPlanGroupRequestModel requestModel)
    {
        var request = new AddPlanToPlanGroupCommand()
        {
            PlanId = requestModel.PlanId,
            PlanGroupId = requestModel.PlanGroupId,
            UserId = UserId
        };

        return await Mediator.Send(request);
    }
}