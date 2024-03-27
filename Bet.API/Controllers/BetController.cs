using Bet.Application.UseCases.Bet.DefineWinner;
using Bet.Application.UseCases.Bet.Pay;
using Bet.Application.UseCases.Bet.Register;
using Bet.Application.UseCases.User.JoinBet;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Bet.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bet.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BetController : BaseBetController
{
    [HttpPost("create")]
    [ProducesResponseType(typeof(ResponseRegisterBet), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterUsuario(
      [FromServices] IRegisterBetUseCase useCase,
      [FromBody] RequestRegisterBet request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpPost("join")]
    [ProducesResponseType(typeof(ResponseRegisterBet), StatusCodes.Status201Created)]
    public async Task<IActionResult> EntrarAposta(
      [FromServices] IJoinBetUseCase useCase,
      [FromBody] RequestJoinBet request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("difine-winner/{id}")]
    [ProducesResponseType(typeof(ResponseRegisterBet), StatusCodes.Status201Created)]
    public async Task<IActionResult> DefinirGanhador(
      [FromServices] IDefineWinner useCase,
      [FromBody] Team request,
      [FromRoute] long id)
    {
        await useCase.Execute(request, id);

        return Ok("Time Ganhador Definido");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("pay")]
    [ProducesResponseType(typeof(ResponseRegisterBet), StatusCodes.Status201Created)]
    public async Task<IActionResult> Pagar(
      [FromServices] IPayBetsUseCase useCase)
    {
        await useCase.Execute();

        return Ok("Apostas Pagas");
    }
}
