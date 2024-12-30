using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Instructors.Commands.LoginInstructorCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.RegisterInstructorCommand;

namespace RandevuPlus.API.App.Features.Instructors
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginInstructorCommandResponse), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<ActionResult<LoginInstructorCommandResponse>> Login([FromBody] LoginInstructorCommnad command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterInstructorCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);
    }
}
