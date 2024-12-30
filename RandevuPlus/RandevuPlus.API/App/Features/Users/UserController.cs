using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Users.Commands.ChangePasswordCommand;
using RandevuPlus.API.App.Features.Users.Commands.LoginCommand;
using RandevuPlus.API.App.Features.Users.Commands.RegisterCommand;

namespace RandevuPlus.API.App.Features.Users
{
    [Route("api/users")]
    public class UserController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginCommandResponse), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<ActionResult<LoginCommandResponse>> Login([FromBody] LoginCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);
    }
}
