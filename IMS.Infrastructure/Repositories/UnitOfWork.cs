using IMS.Application.Abstractions;
using IMS.Infrastructure.Database;

namespace IMS.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
    {
        if (_context.Database.CurrentTransaction != null)
            return await action();

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var result = await action();

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

