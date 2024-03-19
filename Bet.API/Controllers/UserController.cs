using Bet.Application.UseCases.User.Login;
using Bet.Application.UseCases.User.Register;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Bet.API.Controllers;

public class UserController : BaseBetController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUser), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterUsuario(
       [FromServices] IRegisterUserUseCase useCase,
       [FromBody] RequestRegisterUser request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [Route("Login")]
    [HttpPost]
    [ProducesResponseType(typeof(ResponseLogin), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] RequestLogin request,
                                           [FromServices] ILoginUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
