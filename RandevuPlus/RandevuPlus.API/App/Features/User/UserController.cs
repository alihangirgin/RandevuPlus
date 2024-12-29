using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.User.Commands.ChangePasswordCommand;
using RandevuPlus.API.App.Features.User.Commands.LoginCommand;
using RandevuPlus.API.App.Features.User.Commands.RegisterCommand;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RandevuPlus.API.App.Features.User
{
    [Route("api/users")]
    public class UserController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginCommandResponse), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<ActionResult<LoginCommandResponse>> Login([FromBody]LoginCommand command)
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
