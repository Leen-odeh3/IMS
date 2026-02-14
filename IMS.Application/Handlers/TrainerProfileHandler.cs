using IMS.Application.DTOs.Users;
using IMS.Application.Mapper;
using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Interfaces;

namespace IMS.Application.Handlers;
public class TrainerProfileHandler : IRoleProfileHandler
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly UserMapper _mapper;

    public TrainerProfileHandler(
        ITrainerRepository trainerRepository,
        UserMapper mapper)
    {
        _trainerRepository = trainerRepository;
        _mapper = mapper;
    }

    public string Role => StaticRole.Trainer;

    public async Task HandleAsync(RegisterUserDto dto, AppUser user)
    {
        var trainer = _mapper.MapToTrainer(dto);
        trainer.UserId = user.Id;
        await _trainerRepository.AddAsync(trainer);
    }
}