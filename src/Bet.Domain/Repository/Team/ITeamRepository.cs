
namespace Bet.Domain.Repository.Team;
public interface ITeamRepository
{
    Task<Entities.Team> GetByIdAsync(long id);
    Task Add(Entities.Team team);
    void Update(Entities.Team team);
}
