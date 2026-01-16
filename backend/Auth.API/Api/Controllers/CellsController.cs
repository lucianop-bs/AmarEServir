using AmarEServir.Core.Results.Extensions;
using Auth.API.Application.Cells.CreateCell;
using Auth.API.Application.Cells.DeleteCell;
using Auth.API.Application.Cells.GetCellByGuid;
using Auth.API.Application.Cells.Models;
using Auth.API.Application.Cells.UpdateCell;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auth.API.Api.Controllers
{
    [Route("api/cells")]
    [ApiController]
    public class CellsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CellsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedCellResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateCell(CreateCellCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetCell), new { id = result.Value.Id }, result.Value);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CellModelView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCell(Guid id)
        {
            var result = await _mediator.Send(new GetCellByGuidQuery(id));

            return Ok(result);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(CellModelView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCell([FromRoute] Guid id, UpdateCellRequest command)
        {
            var result = await _mediator.Send(new UpdateCellCommand(id, command.Name, command.LiderId));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCell(Guid id)
        {
            var result = await _mediator.Send(new DeleteCellCommand(id));

            return result.ToApiResult(HttpStatusCode.NoContent).ToActionResult();
        }
    }
}
