using Auth.API.Application.Users.CreateUser;
using Auth.API.Application.Users.GetUserByGuid;
using Auth.API.Application.Users.Models;
using MediatR;
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserModelView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var result = await _mediator.Send(new GetUserByGuidQuery(id));

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {

                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);

        }
    }
}
