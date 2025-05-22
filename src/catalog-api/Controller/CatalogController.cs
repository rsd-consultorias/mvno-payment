using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controller;

[ApiController]
[Route("api/catalog")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesErrorResponseType(typeof(ErrorResponse))]
[Authorize]
public class CatalogController : ControllerBase
{

    [HttpGet("plans")]
    [ProducesResponseType(typeof(List<PlanDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<PlanDTO>> GetAllPlans([FromQuery(Name = "type")] String? type, [FromQuery(Name = "service")] String? service)
    {
        // Obtém o usuário autenticado e seus claims
        var user = HttpContext.User;
        var subClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "sub não encontrado";

        // Adiciona o valor do sub ao header da resposta
        Response.Headers.Append("Teste", subClaim);
        return Ok(new List<PlanDTO>());
    }

    [HttpPost("plans")]
    [ProducesResponseType(typeof(PlanDTO), StatusCodes.Status201Created)] // Informa que o status padrão é 201 e retorna um PlanDTO
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<PlanDTO> CreatePlan([FromBody] PlanDTO planRequest)
    {
        return Created($"plans/{Guid.NewGuid()}", planRequest);
    }

    [HttpPut("plans/{plan_id}")]
    [ProducesResponseType(typeof(PlanDTO), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<PlanDTO> UpdatePlan([FromRoute(Name = "plan_id")] Guid planId, [FromBody] PlanDTO planRequest)
    {
        return Accepted($"plans/{Guid.NewGuid()}", planRequest);
    }

    [HttpDelete("plans/{plan_id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult DeletePlan([FromRoute(Name = "plan_id")] Guid planId)
    {
        return Accepted();
    }

    [HttpGet("plans/{plan_id}")]
    [ProducesResponseType(typeof(PlanDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PlanDTO> GetPlanById([FromRoute(Name = "plan_id")] Guid planId)
    {

        return Ok(new PlanDTO());
    }
}