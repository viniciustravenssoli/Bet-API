
using Bet.Application.Services.LoggedUser;
using Bet.Communication.Response;
using Bet.Domain.Repository.UserBet;

namespace Bet.Application.UseCases.Bet.GetAllFromUser;
public class GetAllFromUser : IGetAllFromUser
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserBetWriteOnlyRepository _userBetWriteOnlyRepository;

    public GetAllFromUser(ILoggedUser loggedUser, IUserBetWriteOnlyRepository userBetWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _userBetWriteOnlyRepository = userBetWriteOnlyRepository;
    }

    public async Task<ResponseGetAllBets> Execute(int page, int PageSize)
    {
        var user = await _loggedUser.GetUser();

        var betsFromUser = await _userBetWriteOnlyRepository.GetUserBetsWithPaginationAsync(user.Id, page, PageSize);

        var response = new ResponseGetAllBets(betsFromUser);

        return response;

    }
}
