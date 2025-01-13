using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorExperienceCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.CreateInstructorSkillCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.DeleteInstructorExperienceCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.DeleteInstructorSkillCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.RegisterInstructorCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorExperienceCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorProfileCommand;
using RandevuPlus.API.App.Features.Instructors.Commands.UpdateInstructorSkillCommand;
using RandevuPlus.API.App.Features.Instructors.Queries.GetAppointedUsersQuery;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorProfileQuey;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery;
using RandevuPlus.API.App.Features.Users.Commands.LoginCommand;
using RandevuPlus.API.App.Features.Users.Queries.GetMyAppointedInstructorsQuery;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.App.Features.Instructors
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Result<LoginCommandResponse>>> Register([FromBody] RegisterInstructorCommand command)
            => await _mediator.Send(command);

        [AllowAnonymous]
        [ProducesResponseType(typeof(GetInstructorQueryResponse), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetInstructorQueryResponse>>> GetInstructor(Guid id)
             => await _mediator.Send(new GetInstructorQuery(id));

        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResponse<GetInstructorsQueryResponse>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<Result<PaginatedResponse<GetInstructorsQueryResponse>>>> GetInstructors([FromQuery] int? pageNumber, [FromQuery] int? pageSize, [FromQuery] string? prefix, [FromQuery] DateTime? date, [FromQuery] int? slotStartIndex, [FromQuery] int? slotEndIndex, [FromQuery] int? slotSize, [FromQuery] string? orderBy)
            => await _mediator.Send(new GetInstructorsQuery(pageNumber ?? 1, pageSize ?? 12, prefix, date, slotStartIndex, slotEndIndex, slotSize, true, true, orderBy));

        [ProducesResponseType(typeof(GetInstructorProfileResponse), StatusCodes.Status200OK)]
        [HttpGet("my-profile")]
        public async Task<ActionResult<Result<GetInstructorProfileResponse>>> GetInstructorProfile()
          => await _mediator.Send(new GetInstructorProfileQuery());

        [HttpPatch("my-profile")]
        public async Task<ActionResult<Result>> UpdateInstructorProfile(UpdateInstructorProfileCommand command)
         => await _mediator.Send(command);

        [HttpPost("experience")]
        public async Task<ActionResult<Result>> CreateInstructorExperience([FromBody] CreateInstructorExperienceCommand command)
            => await _mediator.Send(command);

        [HttpPatch("experience/{id}")]
        public async Task<ActionResult<Result>> UpdateInstructorExperience(Guid id, [FromBody] UpdateInstructorExperienceCommand command)
             => await _mediator.Send(command.SetId(id));

        [HttpDelete("experience/{id}")]
        public async Task<ActionResult<Result>> DeleteInstructorExperience(Guid id)
            => await _mediator.Send(new DeleteInstructorExperienceCommand(id));

        [HttpPost("skill")]
        public async Task<ActionResult<Result>> CreateInstructorSkill([FromBody] CreateInstructorSkillCommand command)
            => await _mediator.Send(command);

        [HttpPatch("skill/{id}")]
        public async Task<ActionResult<Result>> UpdateInstructorSkill(Guid id, [FromBody] UpdateInstructorSkillCommand command)
             => await _mediator.Send(command.SetId(id));

        [HttpDelete("skill/{id}")]
        public async Task<ActionResult<Result>> DeleteInstructorSkill(Guid id)
            => await _mediator.Send(new DeleteInstructorSkillCommand(id));

        [HttpGet("my-appointed-users")]
        public async Task<ActionResult<Result<List<GetAppointedUsersQueryResponse>>>> GetMyAppointedUsers()
            => await _mediator.Send(new GetAppointedUsersQuery());
    }
}
