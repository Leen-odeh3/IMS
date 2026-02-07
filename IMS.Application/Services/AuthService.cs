using IMS.Application.Abstractions;
using IMS.Application.DI;
using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace IMS.Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        UserManager<AppUser> userManager,
        IAppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppUser> AddNewUserAsync(
    AppUser appUser,
    string password,
    string role,
    Trainer? trainer,
    Trainee? trainee)
    {
        using var activity =
            Observability.ActivitySource.StartActivity("AuthService.AddNewUser");

        activity?.SetTag("user.email", appUser.Email);
        activity?.SetTag("user.role", role);

        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var isExist = await _userManager.FindByEmailAsync(appUser.Email);

            if (isExist != null)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "User already exists");
                throw new BadRequestException(ErrorMessages.UserAlreadyCreated);
            }

            var result = await _userManager.CreateAsync(appUser, password);

            if (!result.Succeeded)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "Identity creation failed");

                throw new BadRequestException(
                    string.Join(",", result.Errors.Select(e => e.Description)));
            }

            using var roleActivity =
                Observability.ActivitySource.StartActivity("AssignUserRole");

            roleActivity?.SetTag("role", role);

            switch (role?.Trim())
            {
                case StaticRole.Manager:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Manager);
                    break;

                case StaticRole.Trainer:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Trainer);

                    if (trainer != null)
                    {
                        trainer.Id = appUser.Id;
                        trainer.User = appUser;
                        appUser.Trainer = trainer;

                        _context.Trainers.Add(trainer);
                    }
                    break;

                case StaticRole.Trainee:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Trainee);

                    if (trainee != null)
                    {
                        trainee.Id = appUser.Id;
                        trainee.User = appUser;
                        appUser.Trainee = trainee;

                        _context.Trainees.Add(trainee);
                    }
                    break;

                case StaticRole.TeamLead:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.TeamLead);
                    break;

                case StaticRole.Admin:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Admin);
                    break;

                default:
                    roleActivity?.SetStatus(ActivityStatusCode.Error, "Invalid role");
                    throw new BadRequestException(ErrorMessages.InvalidRole);
            }

            await _context.SaveChangesAsync();

            activity?.SetTag("db.saved", true);
            activity?.SetStatus(ActivityStatusCode.Ok);

            return appUser;
        });
    }

}
