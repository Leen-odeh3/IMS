using IMS.Application.DTOs.Users;
using IMS.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace IMS.Application.Mapper;

[Mapper]
public partial class UserMapper
{
    public partial AppUser MapToAppUser(RegisterUserDto dto);

    public partial Trainer MapToTrainer(RegisterUserDto dto);

    public partial Trainee MapToTrainee(RegisterUserDto dto);
    public partial UserResponse MapToResponse(AppUser user);
}