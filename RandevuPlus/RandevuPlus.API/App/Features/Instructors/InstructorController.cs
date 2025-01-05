using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Instructors.Commands.RegisterInstructorCommand;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery;
using RandevuPlus.API.App.Features.Users.Commands.LoginCommand;
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
    }
}
