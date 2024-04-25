using Bet.Domain.Entities;
using Bet.Domain.Repository.Team;

namespace Bet.Infra.Context;
public class TeamRepository : ITeamRepository
{
    private readonly BetContext _context;

    public TeamRepository(BetContext context)
    {
        _context = context;
    }

    public async Task Add(Team team)
    {
        await _context.Teams.AddAsync(team);
    }

    public async Task<Team> GetByIdAsync(long id)
    {
        return await _context.Teams.FindAsync(id);
    }
}
