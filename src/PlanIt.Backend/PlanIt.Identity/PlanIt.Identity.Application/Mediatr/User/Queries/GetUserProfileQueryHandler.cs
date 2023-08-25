using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PlanIt.Identity.Application.Features.Interfaces;
using PlanIt.Identity.Application.Mediatr.Results.Shared;
using PlanIt.Identity.Application.Response;

namespace PlanIt.Identity.Application.Mediatr.User.Queries;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery,Result>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserProfileQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userProfileVm = await _dbContext.Users
            .Where(user => user.Id == request.UserId)
            .Select(user => new UserVm
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                IsEmailConfirmed = user.IsEmailConfirmed
            })
            .FirstAsync(cancellationToken);

        return Result.Create(userProfileVm);
    }
}