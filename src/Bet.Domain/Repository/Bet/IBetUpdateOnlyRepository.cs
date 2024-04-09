namespace Bet.Domain.Repository.Bet;
public interface IBetUpdateOnlyRepository
{
    void Update(Entities.Bet bet);
    Task<Entities.Bet> GetById(long id);
    Task<Dictionary<long, Entities.Bet>> GetNotPaidBetsWithAWinnerAsDictionary();
    Task<Domain.Entities.Bet> GetByIdIncludeUserAndUserBets(long id);

}
