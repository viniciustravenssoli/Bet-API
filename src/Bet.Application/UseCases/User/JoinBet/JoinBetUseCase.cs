using Bet.Application.BaseExceptions;
using Bet.Application.Services.LoggedUser;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Bet.Domain.Entities;
using Bet.Domain.Repository.Bet;
using Bet.Domain.Repository.Team;
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
        private readonly ITeamRepository _teamRepository;

        public JoinBetUseCase(
            ILoggedUser loggedUser,
            IUserBetWriteOnlyRepository userBetRepository,
            IUnitOfWork unitOfWork,
            IBetReadOnlyRepository betReadOnlyRepository,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            ITeamRepository teamRepository)
        {
            _loggedUser = loggedUser ?? throw new ArgumentNullException(nameof(loggedUser));
            _userBetRepository = userBetRepository ?? throw new ArgumentNullException(nameof(userBetRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _betReadOnlyRepository = betReadOnlyRepository ?? throw new ArgumentNullException(nameof(betReadOnlyRepository));
            _userUpdateOnlyRepository = userUpdateOnlyRepository ?? throw new ArgumentNullException(nameof(userUpdateOnlyRepository));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        public async Task<ResponseJoinBet> Execute(RequestJoinBet request)
        {
            var user = await GetUserOrThrowNotFound();
            var bet = await GetBetOrThrowNotFound(request.BetId);
            var betsMadeToday = await _userBetRepository.GetUserBetsMadeTodayAsync(user.Id);

            await Validate(request, bet, user, betsMadeToday);

            var userBets = await _userBetRepository.GetUserBetsByBetIdAsync(request.BetId);
            var (totalAmount, amountOnTeamA, amountOnTeamB) = CalculateAmounts(userBets);
            var userBet = CreateUserBet(user.Id, bet.Id, bet, request.BetAmount, request.ChoseTeamId, totalAmount, amountOnTeamA, amountOnTeamB);

            user.Balance -= userBet.BetAmount;
            await AddUserBetAndCommit(user, userBet);

            var response = new ResponseJoinBet
            {
                Value = userBet.BetAmount,
                PossibleReturn = userBet.BetAmount * userBet.Odd,
                Chose = userBet.ChosenTeamId
            };

            return response;
        }

        private async Task<Domain.Entities.User> GetUserOrThrowNotFound()
        {
            var user = await _loggedUser.GetUser();
            if (user == null)
                throw new NotFoundException("Usuário não encontrado");
            return user;
        }

        private async Task<Domain.Entities.Bet> GetBetOrThrowNotFound(long betId)
        {
            var bet = await _betReadOnlyRepository.GetById(betId);
            if (bet == null)
                throw new NotFoundException("Aposta não encontrada");
            return bet;
        }

        private async Task Validate(RequestJoinBet request, Domain.Entities.Bet bet, Domain.Entities.User user, int betsMadeToday)
        {
            var validator = new JoinBetValidator();
            var result = validator.Validate(request);
            var errorMessages = result.Errors.Select(e => e.ErrorMessage);

            var validationErrors = new List<string>(errorMessages);

            if (bet.Paid)
                validationErrors.Add("Não é possível entrar em apostas já pagas.");

            if (user.Balance < request.BetAmount)
                validationErrors.Add("Saldo insuficiente para realizar a aposta.");

            var team = await _teamRepository.GetByIdAsync(request.ChoseTeamId);
            if (team == null || (team.Id != bet.HomeId && team.Id != bet.VisitorId))
                throw new ConflictException("Time informado não pertence a essa aposta");

            if (user.MaxDailyBets <= betsMadeToday)
                throw new ConflictException("Você atingiu seu limite diário de apostas");

            if (validationErrors.Any())
                throw new ValidationErrorException(validationErrors);
        }

        private (double totalAmount, double amountOnTeamA, double amountOnTeamB) CalculateAmounts(IEnumerable<UserBet> userBets)
        {
            var totalAmount = userBets.Sum(ub => ub.BetAmount);
            var amountOnTeamA = userBets.Where(ub => ub.ChosenTeamId == ub.Bet.HomeId).Sum(ub => ub.BetAmount);
            var amountOnTeamB = userBets.Where(ub => ub.ChosenTeamId == ub.Bet.VisitorId).Sum(ub => ub.BetAmount);
            return (totalAmount, amountOnTeamA, amountOnTeamB);
        }

        private UserBet CreateUserBet(long userId, long betId, Domain.Entities.Bet bet, double betAmount, long chosenTeamId, double totalAmount, double amountOnTeamA, double amountOnTeamB)
        {
            var userBet = new UserBet
            {
                UserId = userId,
                BetId = betId,
                BetAmount = betAmount,
                ChosenTeamId = chosenTeamId,
                Bet = bet
            };

            userBet.CalculateOddOnCreate(totalAmount, amountOnTeamA, amountOnTeamB);
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
