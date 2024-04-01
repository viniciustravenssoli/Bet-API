using Bet.Communication.Request;
using Bet.Domain.Repository.Bet;
using Bet.Infra;

namespace Bet.Application.UseCases.Bet.DefineWinner;
public class DefineWinner : IDefineWinner
{
    private readonly IBetUpdateOnlyRepository _updateBetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DefineWinner(IBetUpdateOnlyRepository updateBetRepository, IUnitOfWork unitOfWork)
    {
        _updateBetRepository = updateBetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestDefineWinner request, long id)
    {
        var bet = await _updateBetRepository.GetById(id);

        bet.Winner = request.Winner;
        _updateBetRepository.Update(bet);

        await _unitOfWork.Commit();
    }
}
