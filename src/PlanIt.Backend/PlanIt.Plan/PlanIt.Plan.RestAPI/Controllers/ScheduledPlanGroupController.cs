using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Mediatr.PlanGroup.Queries.GetPlanGroups;

namespace PlanIt.Plan.RestAPI.Controllers;

public class ScheduledPlanGroupController : ApiControllerBase
{
    /// <summary>
    /// Create Scheduled PlanGroup
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /scheduled-plan-group
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="401">User is not authorized</response>
    /// <response code="400">Request does not have the valid format</response>
    /// <response code="406">Validation error</response>
    [Authorize]
    [HttpPost("scheduled-plan-group")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> CreateScheduledPlanGroup()
    {
        
        
        return Ok();
    }
}