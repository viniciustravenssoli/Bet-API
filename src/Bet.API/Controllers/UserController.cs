using Bet.Application.BaseExceptions;
using Bet.Application.UseCases.User.Login;
using Bet.Application.UseCases.User.Register;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Bet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseBetController
{
    [HttpPost("Register")]
    [ProducesResponseType(typeof(ResponseRegisterUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] RequestLogin request,
                                           [FromServices] ILoginUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
