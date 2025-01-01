using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand;
using RandevuPlus.API.App.Features.Courses.Commands.DeleteCourseCommand;
using RandevuPlus.API.App.Features.Courses.Commands.UpdateCourseCommand;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.App.Features.Courses.Queries.GetMyCoursesQuery;

namespace RandevuPlus.API.App.Features.Courses
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult> CreateCourse([FromBody] CreateCourseCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseCommand command)
            => (await _mediator.Send(command.SetId(id))).ToActionResult(this);

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(Guid id)
            => (await _mediator.Send(new DeleteCourseCommand(id))).ToActionResult(this);

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseQueryResponse>> GetCourse(Guid id)
           => (await _mediator.Send(new GetCourseQuery(id))).ToActionResult(this);

        [HttpGet("my-courses")]
        public async Task<ActionResult<List<GetCourseQueryResponse>>> GetMyCourses([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
            => (await _mediator.Send(new GetMyCoursesQuery(pageNumber ?? 1, pageSize ?? 5))).ToActionResult(this);
    }
}
