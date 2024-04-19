using Bet.Application.BaseExceptions;
using Bet.Application.Services.LoggedUser;
using Bet.Communication.Request;
using Bet.Domain.Repository.User;
using Bet.Infra;

namespace Bet.Application.UseCases.User.ChangeUserData;
public class ChangeUserDataUseCase : IChangeUserDataUseCase
{
    private readonly IUserReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _usuarioRepository;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public ChangeUserDataUseCase(IUserReadOnlyRepository usuarioReadOnlyRepository, IUserWriteOnlyRepository usuarioRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser, IUserUpdateOnlyRepository updateOnlyRepository)
    {
        _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
    }

    public async Task Execute(RequestChangeUserData request)
    {
        var loggedUser = await _loggedUser.GetUser();

        var user = await _updateOnlyRepository.GetById(loggedUser.Id);

        await Validate(request);

        user.Email = request.NewEmail;
        user.Name = request.NewName;
        user.Phone = request.NewPhone;
        user.MaxDailyBets = request.MaxLimitPerDay;

        _updateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }


    private async Task Validate(RequestChangeUserData request)
    {
        var validator = new ChangeUserDataValidator();
        var result = validator.Validate(request);

        var userExists = await _usuarioReadOnlyRepository.ExistUserWithEmail(request.NewEmail);

        if (userExists)
        {
            throw new ConflictException("Email em uso");
        }

        if (!result.IsValid)
        {
            var errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorsMessages);
        }
    }
}
