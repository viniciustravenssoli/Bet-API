using Bet.Communication.Response;

namespace Bet.Application.UseCases.Bet.GetAllOpenWithOdd;
public interface IGetAllOpenWithOdd
{
    Task<List<BetInfo>> Execute(int page, int pageSize);
}
