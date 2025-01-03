using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.InstructorReviews.Commands.MakeReviewCommand;

namespace RandevuPlus.API.App.Features.InstructorReviews
{
    [Route("api/instructor-reviews")]
    [ApiController]
    public class InstructorReviewController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> MakeReview([FromBody] MakeReviewCommand command)
             => (await _mediator.Send(command)).ToActionResult(this);
    }
}
