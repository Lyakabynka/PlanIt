using System.Text.Json.Serialization;

namespace PlanIt.Identity.Domain.Entities;

public class RefreshSession : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RefreshToken { get; set; }

    public User? User { get; set; }
}