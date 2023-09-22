using System.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlanIt.Plan.Application.Features.Interfaces;
using PlanIt.Plan.Application.Response;

namespace PlanIt.Plan.Application.Features.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result>
    where TRequest : IValidatableRequest<Result>
{
    private readonly IValidator<TRequest> _validator;
    private readonly IApplicationDbContext _dbContext;
    public ValidationBehavior(IValidator<TRequest> validator, IApplicationDbContext dbContext)
    {
        _validator = validator;
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.Database
            .BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        
        try
        {
            var validationResult =
                await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                await transaction.CommitAsync(cancellationToken);

                return Result.FormBadRequest(validationResult);
            }

            var response = await next();

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}