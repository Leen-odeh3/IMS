

namespace IMS.Application.DTOs.Users;
public class RegisterUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    // Trainer data
    public int? YearsOfExperience { get; set; }

    // Trainee data
    public int? GraduationYear { get; set; }
    public string? GitHubUsername { get; set; }
    public string? LinkedInProfile { get; set; }
    public string? CVUrl { get; set; }
}
