using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Notifications.Queries.GetNotificationsCountQuery;
using RandevuPlus.API.App.Features.Notifications.Queries.GetNotificationsQuery;

namespace RandevuPlus.API.App.Features.Notifications
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<Result<List<GetNotificationsQueryResponse>>>> GetNotifications()
             => await _mediator.Send(new GetNotificationsQuery());

        [HttpGet("count")]
        public async Task<ActionResult<Result<GetNotificationsCountQueryResponse>>> GetNotificationCount()
            => await _mediator.Send(new GetNotificationsCountQuery());
    }
}
