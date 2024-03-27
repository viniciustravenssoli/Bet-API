using Bet.Domain.Entities;

namespace Bet.Application.Services.LoggedUser;
public interface ILoggedUser
{
    Task<User> GetUser();
}
