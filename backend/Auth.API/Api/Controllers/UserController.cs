using AmarEServir.Core.Results.Extensions;
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

            return result.ToApiResult().ToActionResult();
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserModelView), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
         
            return CreatedAtAction(nameof(GetUser), new { id = result.IsSuccess ? result.Value.Id :Guid.Empty }, result);

        }
    }
}
