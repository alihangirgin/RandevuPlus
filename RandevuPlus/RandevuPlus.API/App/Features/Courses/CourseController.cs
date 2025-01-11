using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Courses.Commands.CreateCourseCommand;
using RandevuPlus.API.App.Features.Courses.Commands.DeleteCourseCommand;
using RandevuPlus.API.App.Features.Courses.Commands.UpdateCourseCommand;
using RandevuPlus.API.App.Features.Courses.Queries.GetCourseQuery;
using RandevuPlus.API.App.Features.Courses.Queries.GetCoursesByInstructorIdQuery;
using RandevuPlus.API.App.Features.Courses.Queries.GetMyCoursesQuery;

namespace RandevuPlus.API.App.Features.Courses
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Result>> CreateCourse([FromBody] CreateCourseCommand command)
            => await _mediator.Send(command);

        [HttpPatch("{id}")]
        public async Task<ActionResult<Result>> UpdateCourse(Guid id, [FromBody] UpdateCourseCommand command)
            => await _mediator.Send(command.SetId(id));

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> DeleteCourse(Guid id)
            => await _mediator.Send(new DeleteCourseCommand(id));

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseQueryResponse>> GetCourse(Guid id)
           => (await _mediator.Send(new GetCourseQuery(id))).ToActionResult(this);

        [HttpGet("my-courses")]
        public async Task<ActionResult<List<GetCourseQueryResponse>>> GetMyCourses([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
            => (await _mediator.Send(new GetMyCoursesQuery(pageNumber ?? 1, pageSize ?? 5))).ToActionResult(this);

        [AllowAnonymous]
        [HttpGet("by-instructorId/{id}")]
        public async Task<ActionResult<Result<List<GetCourseQueryResponse>>>> GetCoursesByInstructorId(Guid id)
            => (await _mediator.Send(new GetCoursesByInstructorIdQuery(id)));
    }
}
