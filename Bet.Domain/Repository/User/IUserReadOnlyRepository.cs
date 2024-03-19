namespace Bet.Domain.Repository.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistUserWithEmail(string email);
    Task<Entities.User> GetByEmailAndPassword(string email, string password);
    Task<Entities.User> GetByEmail(string email);
}
