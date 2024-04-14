using Bet.Application.BaseExceptions;
using Bet.Application.Services.Hash;
using Bet.Application.Services.LoggedUser;
using Bet.Domain.Repository.User;
using Bet.Infra;

namespace Bet.Application.UseCases.User.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repositorio;
    private readonly PasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository repositorio, PasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repositorio = repositorio;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestChangePassword request)
    {
        var loggedUser = await _loggedUser.GetUser();

        var user = await _repositorio.GetById(loggedUser.Id);

        Validate(request, user);

        user.Password = _passwordHasher.Hash(request.NewPassword);

        _repositorio.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePassword request, Domain.Entities.User user)
    {
        var validator = new ChangePasswordValidator();
        var resultado = validator.Validate(request);

        var senhaAtualCriptografada = _passwordHasher.Hash(request.CurrentPassword);

        if (!user.Password.Equals(senhaAtualCriptografada))
        {
            resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("senhaAtual", "Senha atual invalida"));
        }

        if (!resultado.IsValid)
        {
            var mensagens = resultado.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrorException(mensagens);
        }
    }
}
