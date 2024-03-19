using Bet.Application.BaseExceptions;
using Bet.Application.Services.Hash;
using Bet.Application.Services.Token;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Bet.Domain.Repository.User;
using Bet.Infra;


namespace Bet.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _usuarioReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordHasher _passwordHasher;
    private readonly TokenController _tokenController;

    public RegisterUserUseCase(IUserReadOnlyRepository usuarioReadOnlyRepository, IUserWriteOnlyRepository usuarioRepository, IUnitOfWork unitOfWork, PasswordHasher passwordHasher, TokenController tokenController)
    {
        _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenController = tokenController;
    }

    public async Task<ResponseRegisterUser> Execute(RequestRegisterUser request)
    {
        await Validate(request);

        var entity = new Domain.Entities.User
        {
            Name = request.UserName,
            Email = request.Email,
            Password = request.Password,
            Phone = request.Phone,
            
        };

        entity.Password = _passwordHasher.Hash(request.Password);

        await _usuarioRepository.Add(entity);
        await _unitOfWork.Commit();

        var token = _tokenController.GenerateToken(entity.Email);

        return new ResponseRegisterUser
        {
            Token = token,
        };
    }

    private async Task Validate(RequestRegisterUser request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var userExists = await _usuarioReadOnlyRepository.ExistUserWithEmail(request.Email);

        if (userExists)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", "Email Already in Use"));
        }

        if (!result.IsValid)
        {
            var errorsMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errorsMessages);
        }
    }
}
