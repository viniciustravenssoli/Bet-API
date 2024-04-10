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

    public async Task<List<UserBet>> GetUnpaidBets()
    {
        return await _context.UserBets
                .Include(ub => ub.Bet)
                .Where(ub => !ub.Bet.Paid)
                .ToListAsync();
    }

    public async Task<List<UserBet>> GetUserBetsByBetIdAsync(long betId)
    {
        return await _context.UserBets
            .Include(ub => ub.User)
            .Where(ub => ub.BetId == betId)
            .ToListAsync();
    }
    public async Task<List<UserBet>> GetUserBetsWithPaginationAsync(long userId, int page = 1, int pageSize = 10)
    {
        return await _context.UserBets
            .Where(ub => ub.UserId == userId)
            .OrderByDescending(ub => ub.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
