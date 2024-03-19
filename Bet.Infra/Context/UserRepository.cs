using Bet.Domain.Entities;
using Bet.Domain.Repository.User;
using Microsoft.EntityFrameworkCore;

namespace Bet.Infra.Context;
public class UserRepository : IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserWriteOnlyRepository
{
    private readonly BetContext _context;

    public UserRepository(BetContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> ExistUserWithEmail(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email.Equals(email));
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email));
    }

    public async Task<User> GetByEmailAndPassword(string email, string password)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(email) && c.Password.Equals(password));
    }

    public async Task<User> GetById(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }
}
