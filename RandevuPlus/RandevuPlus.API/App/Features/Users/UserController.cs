using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Users.Commands.ChangePasswordCommand;
using RandevuPlus.API.App.Features.Users.Commands.LoginCommand;
using RandevuPlus.API.App.Features.Users.Commands.RegisterCommand;
using RandevuPlus.API.App.Features.Users.Commands.SaveInstructorCommand;
using RandevuPlus.API.App.Features.Users.Commands.UpdateNameCommand;
using RandevuPlus.API.App.Features.Users.Commands.UpdateProfileCommand;
using RandevuPlus.API.App.Features.Users.Queries.GetMyAppointedInstructorsQuery;
using RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery;
using RandevuPlus.API.App.Features.Users.Queries.GetSavedInstructorsQuery;

namespace RandevuPlus.API.App.Features.Users
{
    [Route("api/users")]
    public class UserController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginCommandResponse), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<ActionResult<Result<LoginCommandResponse>>> Login([FromBody] LoginCommand command)
            => (await _mediator.Send(command));
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Result<LoginCommandResponse>>> Register([FromBody] RegisterCommand command)
          => await _mediator.Send(command);

        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);


        [HttpPatch("full-name")]
        public async Task<ActionResult<Result>> UpdateUserName([FromBody] UpdateNameCommand command)
            => await _mediator.Send(command);

        [HttpPost("my-profile")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Result>> UpdateProfile([FromForm] UpdateProfileCommand command)
             => await _mediator.Send(command.SetUrl(Request.Scheme, Request.Host.Value ?? string.Empty));

        [HttpGet("my-profile")]
        public async Task<ActionResult<Result<GetProfileQueryResponse>>> GetProfile()
            => await _mediator.Send(new GetProfileQuery());

        [HttpGet("my-appointed-instructors")]
        public async Task<ActionResult<Result<List<GetMyAppointedInstructorsQueryResponse>>>> GetMyAppointedInstructors()
            => await _mediator.Send(new GetMyAppointedInstructorsQuery());

        [HttpPost("save-instructor")]
        public async Task<ActionResult<Result>> SaveInstructor([FromBody] SaveInstructorCommand command)
            => await _mediator.Send(command);

        [HttpGet("saved-instructors")]
        public async Task<ActionResult<Result<List<GetSavedInstructorsQueryResponse>>>> GetSavedInstructors()
            => await _mediator.Send(new GetSavedInstructorsQuery());
    }
}
