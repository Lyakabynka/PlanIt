using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PlanIt.Identity.Application.Abstractions.Interfaces;
using PlanIt.Identity.Application.Mediator.Results;
using PlanIt.Identity.Application.Mediator.Results.Shared;

namespace PlanIt.Identity.Application.Mediator.User.Queries;

public class GetUserProfileQuery : IRequest<OneOf<Success<UserVm>,BadRequest>>
{
    public Guid UserId { get; set; }
}

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery,OneOf<Success<UserVm>, BadRequest>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserProfileQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OneOf<Success<UserVm>, BadRequest>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (userProfileVm is null) return new BadRequest();

        return new Success<UserVm>(userProfileVm);
    }
}