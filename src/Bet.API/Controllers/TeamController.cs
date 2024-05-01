using Bet.Application.UseCases.Team.RegisterTeam;
using Bet.Application.UseCases.Team.Update;
using Bet.Communication.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bet.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TeamController : BaseBetController
{
    [HttpPost("create")]
    public async Task<IActionResult> RegistrarTime(
      [FromServices] IRegisterTeamUseCase useCase,
      [FromBody] RequestRegisterTeam request)
    {
        await useCase.Execute(request);

        return Created(string.Empty, "Criado com sucesso");
    }

    [HttpPatch]
    [Authorize]
    [Route("update-team/{teamId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AtualizarTime(
        [FromServices] IUpdateTeamUseCase useCase,
        [FromBody] RequestUpdateTeam request,
        [FromRoute] long teamId)
    {
        await useCase.Execute(request, teamId);

        return NoContent();
    }
}
