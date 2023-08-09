using PlanIt.Identity.Domain.Enums;

namespace PlanIt.Identity.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    public bool IsEmailConfirmed { get; set; } = false;

    public List<RefreshSession> RefreshSessions { get; set; }
}




