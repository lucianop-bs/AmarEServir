using AmarEServir.Core.Results.Extensions;
using Auth.API.Application.Cells.Commands.CreateCell;
using Auth.API.Application.Cells.Commands.DeleteCell;
using Auth.API.Application.Cells.Commands.UpdateCell;
using Auth.API.Application.Cells.Queries.GetCellByGuid;
using Auth.API.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(CellResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("ManagementOnly")]
        public async Task<IActionResult> CreateCell([FromBody] CreateCellRequest request)
        {
            var result = await _mediator.Send(new CreateCellCommand(request));

            return CreatedAtRoute("GetCellByGuid", new { id = result.IsSuccess ? result.Value.Id : Guid.Empty }, result);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("ManagementOnly")]
        public async Task<IActionResult> UpdateCell([FromRoute] Guid id, [FromBody] UpdateCellRequest request)
        {
            var result = await _mediator.Send(new UpdateCellCommand(id, request));

            return result.ToApiResult().ToActionResult();
        }

        [HttpGet("{id}", Name = "GetCellByGuid")]
        [ProducesResponseType(typeof(CellResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("ManagementOnly")]
        public async Task<IActionResult> GetCell(Guid id)
        {
            var result = await _mediator.Send(new GetCellByGuidQuery(id));

            return result.ToApiResult().ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("ManagementOnly")]
        public async Task<IActionResult> DeleteCell(Guid id)
        {
            var result = await _mediator.Send(new DeleteCellCommand(id));

            return result.ToApiResult().ToActionResult();
        }
    }
}