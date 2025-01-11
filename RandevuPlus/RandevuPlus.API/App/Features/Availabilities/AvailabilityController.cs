using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Availabilities.Commands.SetAvailabilityCommand;
using RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;

namespace RandevuPlus.API.App.Features.Availabilities
{
    [Route("api/availabilities")]
    [ApiController]
    public class AvailabilityController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("set-availability")]
        public async Task<ActionResult<Result>> SetAvailability([FromBody] SetAvailabilitiesCommand command)
            => await _mediator.Send(command);

        [ProducesResponseType(typeof(List<GetMyAvailabilityQueryResponse>), StatusCodes.Status200OK)]
        [HttpGet("my-availabilities")]
        public async Task<ActionResult<Result<List<GetInstructorQueryAvailabilityResponse>>>> GetMyAvailabilities()
            => await _mediator.Send(new GetMyAvailabilitiesQuery());
    }
}
