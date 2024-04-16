using Bet.Communication.Response;

namespace Bet.Application.UseCases.Bet.GetAllOpenWithOdd;
public interface IGetAllOpenWithOdd
{
    Task<ResponseBetInfo> Execute(int page, int pageSize);
}
