using AmarEServir.Core.Results.Extensions;
using Auth.API.Application.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.API.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var result = await _mediator.Send(request);

            return result.ToApiResult().ToActionResult();
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst("sub")?.Value;

            var userName = User.FindFirst(ClaimTypes.Name)?.Value
                        ?? User.FindFirst("name")?.Value;

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value
                         ?? User.FindFirst("email")?.Value;

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value
                        ?? User.FindFirst("role")?.Value;

            return Ok(new
            {
                Id = userId,
                Name = userName,
                Email = userEmail,
                Role = userRole
            });
        }
    }
}