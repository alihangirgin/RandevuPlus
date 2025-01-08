using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Appointments.Commands.CreateAppointmentCommand;
using RandevuPlus.API.App.Features.Appointments.Queries.CalculatePriceQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery;

namespace RandevuPlus.API.App.Features.Appointments
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [ProducesResponseType(typeof(CreateAppointmentCommandResponse), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<Result<CreateAppointmentCommandResponse>>> CreateAppointment([FromBody] CreateAppointmentCommand command)
            => await _mediator.Send(command);

        [ProducesResponseType(typeof(Result<List<GetAppointmentsQueryResponse>>), StatusCodes.Status200OK)]
        [HttpGet("my-appointments")]
        public async Task<ActionResult<Result<List<GetAppointmentsQueryResponse>>>> GetMyAppointments([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
            => await _mediator.Send(new GetMyAppointmentsQuery(startDate ?? DateTime.UtcNow.AddHours(3).Date, endDate ?? DateTime.UtcNow.AddHours(3).Date.AddDays(1).AddSeconds(-1)));

        [ProducesResponseType(typeof(GetAppointmentQueryResponse), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetAppointmentQueryResponse>>> GetAppointment(Guid id)
             => await _mediator.Send(new GetAppointmentQuery(id));

        [AllowAnonymous]
        [ProducesResponseType(typeof(CalculatePriceQueryResponse), StatusCodes.Status200OK)]
        [HttpPost("calculate-price")]
        public async Task<ActionResult<Result<CalculatePriceQueryResponse>>> CalculatePrice([FromBody] CalculatePriceQuery query)
            => await _mediator.Send(query);
    }
}
