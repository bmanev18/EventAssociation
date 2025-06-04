using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqliteDmPersistence.Shared;

public class EfcUnitOfWork : IUnitOfWork
{
    private readonly DmContext _context;
 
    public EfcUnitOfWork(DmContext context)
    {
        _context = context;
    }
 
    public async Task<Result<None>> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return Result<None>.Ok(None.Value);
        }
        catch (Exception ex)
        {
            return Result<None>.Err(new Error("0", $"Error during SaveChanges: {ex.Message}"));
        }
    }
}