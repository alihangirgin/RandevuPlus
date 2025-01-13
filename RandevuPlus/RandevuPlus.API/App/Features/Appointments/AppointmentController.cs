using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Appointments.Commands.CreateAppointmentCommand;
using RandevuPlus.API.App.Features.Appointments.Queries.CalculatePriceQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetAppointmentQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsHistoryQuery;
using RandevuPlus.API.App.Features.Appointments.Queries.GetMyAppointmentsQuery;
using RandevuPlus.API.Shared.Dtos;

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

        [ProducesResponseType(typeof(Result<PaginatedResponse<GetMyAppointmentsHistoryQueryResponse>>), StatusCodes.Status200OK)]
        [HttpGet("my-appointments-history")]
        public async Task<ActionResult<Result<PaginatedResponse<GetMyAppointmentsHistoryQueryResponse>>>> GetMyAppointmentsHistory([FromQuery] int? pageNumber, [FromQuery] int? pageSize, [FromQuery] string? prefix, [FromQuery] string? relatedId, [FromQuery] string? status, [FromQuery] string? orderBy, [FromQuery] bool descending)
            => await _mediator.Send(new GetMyAppointmentsHistoryQuery(pageNumber ?? 1 , pageSize ?? 10,prefix, relatedId, status, orderBy, descending));
    }
}
