using Bet.Application.Services.Token;
using Bet.Domain.Entities;
using Bet.Domain.Repository.User;
using Microsoft.AspNetCore.Http;

namespace Bet.Application.Services.LoggedUser;
public class LoggedUser : ILoggedUser
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _usuarioRepository;

    public LoggedUser(IHttpContextAccessor contextAccessor, TokenController tokenController, IUserReadOnlyRepository usuarioRepository)
    {
        _contextAccessor = contextAccessor;
        _tokenController = tokenController;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<User> GetUser()
    {
        var authorization = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var token = authorization["Bearer".Length..].Trim();

        var userEmail = _tokenController.GetEmail(token);

        var user = await _usuarioRepository.GetByEmail(userEmail);

        return user;
    }
}
