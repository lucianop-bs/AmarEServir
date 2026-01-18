using AmarEServir.Core.Results.Extensions;
using Auth.API.Application.Cells.CreateCell;
using Auth.API.Application.Cells.DeleteCell;
using Auth.API.Application.Cells.GetCellByGuid;
using Auth.API.Application.Cells.Models;
using Auth.API.Application.Cells.UpdateCell;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(CreateCellCommand), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateCell(CreateCellCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtRoute("GetCellByGuid", new { id = result.IsSuccess ? result.Value.Id : Guid.Empty }, result.ToApiResult().ToActionResult());
        }

        [HttpGet("{id}", Name = "GetCellByGuid")]
        [ProducesResponseType(typeof(CellModelView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCell(Guid id)
        {
            var result = await _mediator.Send(new GetCellByGuidQuery(id));

            return result.ToApiResult().ToActionResult();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(UpdateCellRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCell([FromRoute] Guid id, [FromBody] UpdateCellRequest command)
        {
            var result = await _mediator.Send(new UpdateCellCommand(id, command.Name, command.LiderId));

            return result.ToApiResult().ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCell(Guid id)
        {
            var result = await _mediator.Send(new DeleteCellCommand(id));

            return result.ToApiResult().ToActionResult();
        }
    }
}
