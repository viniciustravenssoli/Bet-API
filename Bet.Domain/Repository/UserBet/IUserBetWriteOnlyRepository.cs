namespace Bet.Domain.Repository.UserBet;
public interface IUserBetWriteOnlyRepository
{
    Task Add(Entities.UserBet userBet);
    Task<List<Entities.UserBet>> GetUserBetsByBetIdAsync(long betId);
}
