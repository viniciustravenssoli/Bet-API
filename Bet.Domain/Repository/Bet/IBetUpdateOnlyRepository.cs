namespace Bet.Domain.Repository.Bet;
public interface IBetUpdateOnlyRepository
{
    void Update(Entities.Bet bet);
    Task<Entities.Bet> GetById(long id);
    Task<Dictionary<long, Domain.Entities.Bet>> GetNotPaidBetsAsDictionary();

}
