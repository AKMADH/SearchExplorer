using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchExplorer.Application.Commands;
using SearchExplorer.Core.Models;
using System.Threading.Tasks;

namespace SearchExplorer.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
                return BadRequest(new { Message = "Username or password is missing" });

            try
            {
                var token = await _mediator.Send(new LoginCommand(loginModel.Username, loginModel.Password));
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }
        }
    }
}
