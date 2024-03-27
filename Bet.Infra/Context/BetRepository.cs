using Bet.Domain.Repository.Bet;
using Microsoft.EntityFrameworkCore;

namespace Bet.Infra.Context;
public class BetRepository : IBetReadOnlyRepository, IBetUpdateOnlyRepository, IBetWriteOnlyRepository
{
    private readonly BetContext _context;

    public BetRepository(BetContext context)
    {
        _context = context;
    }

    public async Task Add(Domain.Entities.Bet bet)
    {
        await _context.Bets.AddAsync(bet);
    }

    public Task<IList<Domain.Entities.Bet>> GetAllFromUser(long userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Domain.Entities.Bet> GetById(long id)
    {
        return await _context.Bets.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Domain.Entities.Bet>> GetNotPaidBets()
    {
        var unpaidBets = await _context.Bets
        .Include(b => b.UserBets)
        .ThenInclude(ub => ub.User)
        .Where(b => !b.Paid)
        .ToListAsync();

        return unpaidBets;
    }

    public void Update(Domain.Entities.Bet bet)
    {
        _context.Bets.Update(bet);
    }
}
