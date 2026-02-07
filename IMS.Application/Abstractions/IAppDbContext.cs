using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace IMS.Application.Abstractions;
public interface IAppDbContext
{
    DbSet<Trainee> Trainees { get; }
    DbSet<Trainer> Trainers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}