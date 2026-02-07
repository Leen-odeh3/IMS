using IMS.Application.Abstractions;
using IMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;


namespace IMS.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
    {
        try
        {
            if (_context.Database.CurrentTransaction != null)
            {
                return await action();
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var result = await action();

            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }
}
