namespace Bet.Domain.Repository.Bet;
public interface IBetReadOnlyRepository
{
    Task<IList<Entities.Bet>> GetAllFromUser(long userId);
    Task<Entities.Bet> GetById(long betId);
    Task<List<Entities.Bet>> GetUnpaidBetsWithUserBets();
}
