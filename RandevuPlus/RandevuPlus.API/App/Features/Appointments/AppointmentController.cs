using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Appointments.Commands.CreateAppointmentCommand;
using RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery;

namespace RandevuPlus.API.App.Features.Appointments
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [ProducesResponseType(typeof(List<GetAppointmentQueryResponse>), StatusCodes.Status200OK)]
        [HttpGet("my-appointments")]
        public async Task<ActionResult<List<GetAppointmentQueryResponse>>> GetMyAppointments([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
            => (await _mediator.Send(new GetMyAppointmentsQuery(startDate ?? DateTime.UtcNow.Date, endDate ?? DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1)))).ToActionResult(this);

        [ProducesResponseType(typeof(GetAppointmentQueryResponse), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAppointmentQueryResponse>> GetAppointment(Guid id)
             => (await _mediator.Send(new GetAppointmentQuery(id))).ToActionResult(this);
    }
}
