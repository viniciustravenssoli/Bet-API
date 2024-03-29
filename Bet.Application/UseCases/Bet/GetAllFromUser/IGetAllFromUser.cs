using Bet.Communication.Response;

namespace Bet.Application.UseCases.Bet.GetAllFromUser;
public interface IGetAllFromUser
{
    Task<ResponseGetAllBets> Execute(int page, int PageSize);
}
