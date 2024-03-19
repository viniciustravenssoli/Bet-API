using Bet.Domain.Repository.Bet;

namespace Bet.Infra.Context;
public class BetRepository : IBetReadOnlyRepository, IBetUpdateOnlyRepository, IBetWriteOnlyRepository
{
    public Task Add(Domain.Entities.Bet bet)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Domain.Entities.Bet>> GetAllFromUser(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Entities.Bet> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public void Update(Domain.Entities.Bet bet)
    {
        throw new NotImplementedException();
    }
}
