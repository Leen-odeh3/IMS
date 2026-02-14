using IMS.Application.DTOs.Users;
using IMS.Application.Mapper;
using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Interfaces;

namespace IMS.Application.Handlers;
public class TraineeProfileHandler : IRoleProfileHandler
{
    private readonly ITraineeRepository _traineeRepository;
    private readonly UserMapper _mapper;

    public TraineeProfileHandler(
        ITraineeRepository traineeRepository,
        UserMapper mapper)
    {
        _traineeRepository = traineeRepository;
        _mapper = mapper;
    }

    public string Role => StaticRole.Trainee;

    public async Task HandleAsync(RegisterUserDto dto, AppUser user)
    {
        var trainee = _mapper.MapToTrainee(dto);
        trainee.UserId = user.Id;
        await _traineeRepository.AddAsync(trainee);
    }
}