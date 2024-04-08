using Bet.Application.BaseExceptions;
using Bet.Application.Services.Hash;
using Bet.Application.Services.Token;
using Bet.Application.UseCases.User.Register;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Bet.Domain.Repository.User;

namespace Bet.Application.UseCases.User.Login;
public class LoginUseCase : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly TokenController _tokenController;

    public LoginUseCase(IUserReadOnlyRepository usuarioReadOnlyRepository, PasswordHasher passwordHasher, TokenController tokenController)
    {
        _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        _passwordHasher = passwordHasher;
        _tokenController = tokenController;
    }

    public async Task<ResponseLogin> Execute(RequestLogin request)
    {
        await Validate(request);

        var encryptedPassword = _passwordHasher.Hash(request.Password);

        var user = await _usuarioReadOnlyRepository.GetByEmailAndPassword(request.Email, encryptedPassword);

        if (user == null)
        {
            throw new LoginInvalidException();
        }

        return new ResponseLogin
        {
            Name = user.Name,
            Token = _tokenController.GenerateToken(request.Email, user.Role),
        };
    }
    private async Task Validate(RequestLogin request)
    {
        var validator = new LoginUserValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorsMessages);
        }
    }
}
