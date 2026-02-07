using FluentValidation;
using IMS.Application.DTOs.Users;

namespace IMS.Application.Validations;
public class AppUserValidator
    : AbstractValidator<RegisterUserDto>
{
    public AppUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email is required")
            .EmailAddress()
                .WithMessage("Email format is not valid")
            .Must(email => email.EndsWith("@quizplus.com"))
                .WithMessage("Email must be a company email (@quizplus.com)");

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password is required")
            .MinimumLength(3)
                .WithMessage("Password must be at least 3 characters")
            .MaximumLength(10)
                .WithMessage("Password must not exceed 10 characters");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required");

        When(x => x.Role != null && x.Role.ToLower() == "trainee", () =>
        {
            RuleFor(x => x.GraduationYear)
                .NotNull()
                    .WithMessage("Graduation year is required for trainees")
                .InclusiveBetween(2024, DateTime.Now.Year)
                    .WithMessage($"Graduation year must be between 2024 and {DateTime.Now.Year}");
        });
    }
}
