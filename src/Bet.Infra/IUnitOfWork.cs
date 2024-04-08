namespace Bet.Infra;
public interface IUnitOfWork
{
    Task Commit();
}
