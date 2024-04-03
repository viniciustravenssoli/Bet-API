using Bet.Application.BaseExceptions;
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
        await Validate(request);
        var bet = await _updateBetRepository.GetById(id) ?? throw new NotFoundException("Aposta não encontrada"); 

        if (bet.Winner is not null) 
        {
            throw new ValidationErrorException(new List<string> { "O ganhador dessa aposta ja foi definido e não é possivel altera-lo atraves desse recurso" });
        }

        bet.Winner = request.Winner;
        _updateBetRepository.Update(bet);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestDefineWinner request)
    {
        var validator = new DefineWinnerValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorsMessages);
        }

    }
}
