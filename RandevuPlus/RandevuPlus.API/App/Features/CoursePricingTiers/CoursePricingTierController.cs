using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.CoursePricingTiers.Commands.CreateCoursePricingTier;
using RandevuPlus.API.App.Features.CoursePricingTiers.Commands.DeleteCoursePricingTier;
using RandevuPlus.API.App.Features.CoursePricingTiers.Commands.UpdateCoursePricingTier;
using RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTier;
using RandevuPlus.API.App.Features.CoursePricingTiers.Queries.GetCoursePricingTiersByCourse;

namespace RandevuPlus.API.App.Features.CoursePricingTiers
{
    [Route("api/course-pricing-tiers")]
    [ApiController]
    public class CoursePricingTierController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> CreateCoursePricingTier([FromBody] CreateCoursePricingTierCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCoursePricingTier(Guid id, [FromBody] UpdateCoursePricingTierCommand command)
            => (await _mediator.Send(command.SetId(id))).ToActionResult(this);

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCoursePricingTier(Guid id)
            => (await _mediator.Send(new DeleteCoursePricingTierCommand(id))).ToActionResult(this);

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCoursePricingTierResponse>> GetCoursePricingTier(Guid id)
           => (await _mediator.Send(new GetCoursePricingTierQuery(id))).ToActionResult(this);

        [HttpGet("by-course/{id}")]
        public async Task<ActionResult<List<GetCoursePricingTierResponse>>> GetMyCourses(Guid id)
           => (await _mediator.Send(new GetCoursePricingTiersByCourseQuery(id))).ToActionResult(this);
    }
}
