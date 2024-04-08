namespace Bet.Domain.Repository.User;
public interface IUserUpdateOnlyRepository
{
    void Update(Entities.User user);
    Task UpdateBalance(long userId, double newBalance);
    Task BulkUpdateBalanceAsync(long userId, double earnedValue);
    Task<Entities.User> GetById(long id);
    Task UpdateAsync(Entities.User user);
}
