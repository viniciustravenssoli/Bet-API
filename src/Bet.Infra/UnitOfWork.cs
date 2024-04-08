using Bet.Infra.Context;

namespace Bet.Infra;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly BetContext _context;
    private bool _disposed;
    public UnitOfWork(BetContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}
