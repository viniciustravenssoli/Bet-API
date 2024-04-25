using Bet.Application.UseCases.Team.RegisterTeam;
using Bet.Communication.Request;
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
}
