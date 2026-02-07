using Microsoft.AspNetCore.Identity;

namespace IMS.Core.Entities;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; } = false;
    public string? ProfilePictureUrl { get; set; }
    public string? Specialization { get; set; }
    public string? University { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;    
    public Trainer? Trainer { get; set; }
    public Trainee? Trainee { get; set; }    
}
