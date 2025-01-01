using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Purchases.Commands;

namespace RandevuPlus.API.App.Features.Purchases
{
    [Route("api/purchases")]
    [ApiController]
    public class PurchaseController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("{id}")]
        public async Task<ActionResult> PurcheseAppointment(Guid id)
            => (await _mediator.Send(new PurchaseAppointmentCommand(id))).ToActionResult(this);
    }
}
