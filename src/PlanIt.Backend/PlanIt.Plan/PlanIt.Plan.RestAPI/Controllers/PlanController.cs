using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanIt.Plan.Application.Mediator.Plan.Commands;
using PlanIt.Plan.Application.Mediator.Plan.Queries;
using PlanIt.Plan.RestAPI.Models;

namespace PlanIt.Plan.RestAPI.Controllers;

public class PlanController : ApiControllerBase
{
    [Authorize]
    [HttpPost("plan")]
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

    [Authorize]
    [HttpPut("plan")]
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
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid());
    }

    [Authorize]
    [HttpDelete("plan/{id:guid}")]
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

    [Authorize]
    [HttpGet("plans")]
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

    [Authorize]
    [HttpPost("plan/{id:guid}/schedule/instant")]
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

    [Authorize]
    [HttpPost("plan/{id:guid}/schedule/one-off")]
    public async Task<IActionResult> ScheduleOneOffPlan([FromRoute] Guid id,
        [FromBody] ScheduleOneOffPlanRequestModel requestModelModel)
    {
        var request = new ScheduleOneOffPlanCommand()
        {
            PlanId = id,
            ExecuteUtc = requestModelModel.ExecuteUtc,
            UserId = UserId
        };

        var result = await Mediator.Send(request);
        return result.Match<IActionResult>(
            success => Ok(),
            notFound => NotFound(),
            forbidden => Forbid(),
            badRequest => BadRequest(badRequest.Error));
    }


    [Authorize]
    [HttpPost("plan/{id:guid}/schedule/recurring")]
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
}