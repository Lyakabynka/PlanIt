namespace PlanIt.Identity.Domain.Entities;

public class UserData : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public string AvatarPath { get; set; } = 
        "https://herrmans.de/wp-content/uploads/2019/01/765-default-avatar.png";
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    
    public string? Phone { get; set; }
    
    public string? HomeAddress { get; set; }

    public DateOnly BirthDate { get; set; }
}