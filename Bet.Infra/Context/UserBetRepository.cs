using Bet.Domain.Entities;
using Bet.Domain.Repository.UserBet;
using Microsoft.EntityFrameworkCore;

namespace Bet.Infra.Context;
public class UserBetRepository : IUserBetWriteOnlyRepository
{
    private readonly BetContext _context;

    public UserBetRepository(BetContext context)
    {
        _context = context;
    }

    public async Task Add(UserBet userBet)
    {
        await _context.UserBets.AddAsync(userBet);
    }

    public async Task<List<UserBet>> GetUserBetsByBetIdAsync(long betId)
    {
        return await _context.UserBets
            .Include(ub => ub.User)
            .Where(ub => ub.BetId == betId)
            .ToListAsync();
    }
}
