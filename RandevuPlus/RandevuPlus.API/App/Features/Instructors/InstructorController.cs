using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RandevuPlus.API.App.Features.Instructors
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController(IMediator mediator) : BaseController
    {
    }
}
