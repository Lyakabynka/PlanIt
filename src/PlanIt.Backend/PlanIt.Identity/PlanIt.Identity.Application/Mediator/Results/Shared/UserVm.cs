using PlanIt.Identity.Domain.Enums;

namespace PlanIt.Identity.Application.Mediator.Results.Shared;

public class UserVm
{
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public UserRole Role { get; set; }
    
    public bool IsEmailConfirmed { get; set; }
}