using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Instructors.Commands.RegisterInstructorCommand;

namespace RandevuPlus.API.App.Features.Instructors
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterInstructorCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);
    }
}
