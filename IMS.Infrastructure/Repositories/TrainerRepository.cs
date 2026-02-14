using IMS.Core.Entities;
using IMS.Core.Interfaces;
using IMS.Infrastructure.Database;

namespace IMS.Infrastructure.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private readonly AppDbContext _context;

    public TrainerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Trainer trainer)
    {
        await _context.Trainers.AddAsync(trainer);
    }
}