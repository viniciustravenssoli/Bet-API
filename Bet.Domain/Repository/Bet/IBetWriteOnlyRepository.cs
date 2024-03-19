namespace Bet.Domain.Repository.Bet;
public interface IBetWriteOnlyRepository
{
    Task Add(Entities.Bet bet);
}
