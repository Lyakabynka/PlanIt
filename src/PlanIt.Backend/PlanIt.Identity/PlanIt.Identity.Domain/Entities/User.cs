using PlanIt.Identity.Domain.Enums;

namespace PlanIt.Identity.Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; } = UserRole.User;

    public bool IsEmailConfirmed { get; set; } = false;

    public UserData? UserData { get; set; }

    public List<RefreshSession> RefreshSessions { get; set; }
}




