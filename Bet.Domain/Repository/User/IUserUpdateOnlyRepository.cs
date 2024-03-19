namespace Bet.Domain.Repository.User;
public interface IUserUpdateOnlyRepository
{
    void Update(Entities.User user);
    Task<Entities.User> GetById(long id);
}
