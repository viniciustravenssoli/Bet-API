namespace Bet.Domain.Repository.UserBet;
public interface IUserBetWriteOnlyRepository
{
    Task Add(Entities.UserBet userBet);
    Task<List<Entities.UserBet>> GetUserBetsByBetIdAsync(long betId);
    Task<List<Entities.UserBet>> GetUserBetsWithPaginationAsync(long userId, int page, int pageSize);
    Task<List<Entities.UserBet>> GetUnpaidBets();
    Task<int> GetUserBetsMadeTodayAsync(long userId);
}
