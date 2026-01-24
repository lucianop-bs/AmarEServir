using AmarEServir.Core.Results.Extensions;
using Auth.API.Application.Users.CreateUser;
using Auth.API.Application.Users.DeleteUser;
using Auth.API.Application.Users.GetUserByGuid;
using Auth.API.Application.Users.GetUsersByQuery;
using Auth.API.Application.Users.Models;
using Auth.API.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _mediator.Send(new CreateUserCommand(request));

            return result.ToApiResult(System.Net.HttpStatusCode.Created).ToActionResult();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(UpdateUserRequest), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, request));

            return result.ToApiResult().ToActionResult();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await _mediator.Send(new GetUserByGuidQuery(id));

            return result.ToApiResult().ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));

            return result.ToApiResult().ToActionResult();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("AdminOnly")]
        public async Task<IActionResult> GetUsersByQuery([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? term = null)
        {
            var result = await _mediator.Send(new GetUsersQuery(page, pageSize, term));

            return result.ToApiResult().ToActionResult();
        }
    }
}