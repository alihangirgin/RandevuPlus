using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Availabilities.Commands.SetAvailabilityCommand;
using RandevuPlus.API.App.Features.Availabilities.Queries.GetMyAvailabilitiesQuery;

namespace RandevuPlus.API.App.Features.Availabilities
{
    [Route("api/availabilities")]
    [ApiController]
    public class AvailabilityController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("set-availability")]
        public async Task<ActionResult> SetAvailability([FromBody] SetAvailabilitiesCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [ProducesResponseType(typeof(List<GetMyAvailabilityQueryResponse>), StatusCodes.Status200OK)]
        [HttpGet("my-availabilities")]
        public async Task<ActionResult<List<GetMyAvailabilityQueryResponse>>> GetMyAvailabilities([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
            => (await _mediator.Send(new GetMyAvailabilitiesQuery(startDate ?? DateTime.UtcNow.Date, endDate ?? DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1)))).ToActionResult(this);
    }
}
