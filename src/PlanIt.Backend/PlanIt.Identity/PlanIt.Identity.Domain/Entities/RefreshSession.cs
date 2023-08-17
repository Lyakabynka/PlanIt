using System.Text.Json.Serialization;
using PlanIt.Identity.Domain.Enums;

namespace PlanIt.Identity.Domain.Entities;

public class RefreshSession : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RefreshToken { get; set; }
    
    public string UserAgent { get; set; }
    
    public User? User { get; set; }
}