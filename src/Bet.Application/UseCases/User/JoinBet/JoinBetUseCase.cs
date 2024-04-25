using Bet.Application.BaseExceptions;
using Bet.Application.Services.LoggedUser;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Bet.Domain.Entities;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.User;
using Bet.Domain.Repository.UserBet;
using Bet.Infra;

namespace Bet.Application.UseCases.User.JoinBet
{
    public class JoinBetUseCase : IJoinBetUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserBetWriteOnlyRepository _userBetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBetReadOnlyRepository _betReadOnlyRepository;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

        public JoinBetUseCase(
            ILoggedUser loggedUser,
            IUserBetWriteOnlyRepository userBetRepository,
            IUnitOfWork unitOfWork,
            IBetReadOnlyRepository betReadOnlyRepository,
            IUserUpdateOnlyRepository userUpdateOnlyRepository)
        {
            _loggedUser = loggedUser ?? throw new ArgumentNullException(nameof(loggedUser));
            _userBetRepository = userBetRepository ?? throw new ArgumentNullException(nameof(userBetRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _betReadOnlyRepository = betReadOnlyRepository ?? throw new ArgumentNullException(nameof(betReadOnlyRepository));
            _userUpdateOnlyRepository = userUpdateOnlyRepository ?? throw new ArgumentNullException(nameof(userUpdateOnlyRepository));
        }

        public async Task<ResponseJoinBet> Execute(RequestJoinBet request)
        {
            var user = await _loggedUser.GetUser() ?? throw new NotFoundException("Usuário não encontrado");

            var bet = await _betReadOnlyRepository.GetById(request.BetId) ?? throw new NotFoundException("Aposta não encontrada");

            var betsMadeToday = await _userBetRepository.GetUserBetsMadeTodayAsync(user.Id);

            await Validate(request, bet, user, betsMadeToday);

            var userBets = await _userBetRepository.GetUserBetsByBetIdAsync(request.BetId);

            var (totalAmount, amountOnTeamA, amountOnTeamB) = CalculateAmounts(userBets);

            var userBet = CreateUserBet(user.Id, bet.Id, request.BetAmount, request.ChoseTeamId, totalAmount, amountOnTeamA, amountOnTeamB);

            user.Balance -= userBet.BetAmount;

            await AddUserBetAndCommit(user, userBet);

            var response = new ResponseJoinBet
            {
                Value = userBet.BetAmount,
                PossibleReturn = userBet.BetAmount * userBet.Odd,
                Chose = userBet.ChosenTeam
            };

            return response;
        }

        private async Task Validate(RequestJoinBet request, Domain.Entities.Bet bet, Domain.Entities.User user, int betsMadeToday)
        {
            var validator = new JoinBetValidator();
            var result = validator.Validate(request);

            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            var validationErrors = new List<string>();

            validationErrors.AddRange(errorMessages);

            validationErrors.AddRange(
                bet.Paid ? new[] { "Não é possível entrar em apostas já pagas." } : Enumerable.Empty<string>()
            );

            validationErrors.AddRange(
                user.Balance < request.BetAmount ? new[] { "Saldo insuficiente para realizar a aposta." } : Enumerable.Empty<string>()
            );

            if (user.MaxDailyBets <= betsMadeToday)
            {
                throw new ConflictException("Você atingiu seu limite diário de apostas");
            }

            if (validationErrors.Any())
            {
                throw new ValidationErrorException(validationErrors);
            }
        }


        private (double totalAmount, double amountOnTeamA, double amountOnTeamB) CalculateAmounts(IEnumerable<UserBet> userBets)
        {
            var totalAmount = userBets.Sum(ub => ub.BetAmount);
            var amountOnTeamA = userBets.Where(ub => ub.ChosenTeam == ub.Bet.Visitor).Sum(ub => ub.BetAmount);
            var amountOnTeamB = userBets.Where(ub => ub.ChosenTeam == ub.Bet.Home).Sum(ub => ub.BetAmount);
            return (totalAmount, amountOnTeamA, amountOnTeamB);
        }

        private UserBet CreateUserBet(long userId, long betId, double betAmount, long chosenTeamId, double totalAmount, double amountOnTeamA, double amountOnTeamB)
        {
            var userBet = new UserBet
            {
                UserId = userId,
                BetId = betId,
                BetAmount = betAmount,
                ChosenTeamId = chosenTeamId

            };

            userBet.CalculateOdd(totalAmount, amountOnTeamA, amountOnTeamB);
            return userBet;
        }

        private async Task AddUserBetAndCommit(Domain.Entities.User user, UserBet userBet)
        {
            await _userBetRepository.Add(userBet);
            await _userUpdateOnlyRepository.UpdateBalance(user.Id, user.Balance);
            await _unitOfWork.Commit();
        }
    }
}
