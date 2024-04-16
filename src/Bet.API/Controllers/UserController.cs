using Bet.Application.UseCases.User.ChangePassword;
using Bet.Application.UseCases.User.ChangeUserData;
using Bet.Application.UseCases.User.Login;
using Bet.Application.UseCases.User.Register;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseBetController
{
    [HttpPost("Register")]
    [ProducesResponseType(typeof(ResponseRegisterUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUsuario(
       [FromServices] IRegisterUserUseCase useCase,
       [FromBody] RequestRegisterUser request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpPost]
    [Route("Login")]
    [ProducesResponseType(typeof(ResponseLogin), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] RequestLogin request,
                                           [FromServices] ILoginUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    [Route("alterar-senha")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AlterarSenha(
            [FromServices] IChangePasswordUseCase useCase,
            [FromBody] RequestChangePassword request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpPatch]
    [Authorize]
    [Route("alterar-dados")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AlterarDados(
        [FromServices] IChangeUserDataUseCase useCase,
        [FromBody] RequestChangeUserData request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

}
