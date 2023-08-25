using PlanIt.Identity.Domain.Enums;

namespace PlanIt.Identity.Application.Mediatr.Results.Shared;

public class UserVm
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public UserRole Role { get; set; }
    
    public bool IsEmailConfirmed { get; set; }
    
    public string UserAgent { get; set; }
}