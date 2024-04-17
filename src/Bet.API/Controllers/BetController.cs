using Bet.Application.UseCases.Bet.DefineWinner;
using Bet.Application.UseCases.Bet.GetAllFromUser;
using Bet.Application.UseCases.Bet.GetAllOpenWithOdd;
using Bet.Application.UseCases.Bet.Pay;
using Bet.Application.UseCases.Bet.PayById;
using Bet.Application.UseCases.Bet.Register;
using Bet.Application.UseCases.User.JoinBet;
using Bet.Communication.Request;
using Bet.Communication.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bet.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BetController : BaseBetController
{
    [HttpPost("create")]
    [ProducesResponseType(typeof(ResponseRegisterBet), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterAposta(
      [FromServices] IRegisterBetUseCase useCase,
      [FromBody] RequestRegisterBet request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [Authorize]
    [HttpPost("join")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseJoinBet), StatusCodes.Status201Created)]
    public async Task<IActionResult> EntrarAposta(
      [FromServices] IJoinBetUseCase useCase,
      [FromBody] RequestJoinBet request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("difine-winner/{id}")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DefinirGanhador(
      [FromServices] IDefineWinner useCase,
      [FromBody] RequestDefineWinner request,
      [FromRoute] long id)
    {
        await useCase.Execute(request, id);

        return Ok("Time Ganhador Definido");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("pay")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Pagar(
      [FromServices] IPayBetsUseCase useCase)
    {
        await useCase.Execute();

        return Ok("Apostas Pagas");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("pay/{id}")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PagarPorId(
      [FromServices] IPayBetsByIdUseCase useCase,
      [FromRoute] long id)
    {
        await useCase.Execute(id);

        return Ok("Apostas Pagas");
    }

    [Authorize]
    [HttpGet("pegar-todas")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseGetAllBets), StatusCodes.Status200OK)]
    public async Task<IActionResult> PegarApostas(
      [FromServices] IGetAllFromUser useCase,
      [FromQuery] PageQuery request)
    {
        var result = await useCase.Execute(request.Skip, request.Top);

        return Ok(result);
    }

    [HttpGet("pegar-aposta-em-aberto")]
    [ProducesResponseType(typeof(ResponseBetInfo), StatusCodes.Status200OK)]
    public async Task<IActionResult> PegarApostaEmAberto(
    [FromServices] IGetAllOpenWithOdd useCase,
    [FromQuery] PageQuery request)
    {
        var result = await useCase.Execute(request.Skip, request.Top);

        return Ok(result);
    }
}
