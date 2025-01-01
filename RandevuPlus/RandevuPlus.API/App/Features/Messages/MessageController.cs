using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Messages.Commands.DeleteMessageCommand;
using RandevuPlus.API.App.Features.Messages.Commands.SendMessageCommand;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxCountQuery;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;
using RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery;
using RandevuPlus.API.App.Features.Messages.Queries.GetSendboxQuery;
using RandevuPlus.API.App.Features.Messages.Queries.SearchFriendsQuery;

namespace RandevuPlus.API.App.Features.Messages
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(Guid id)
            => (await _mediator.Send(new DeleteMessageCommand(id))).ToActionResult(this);

        [HttpPost]
        public async Task<ActionResult> SendMessage([FromBody] SendMessageCommand command)
            => (await _mediator.Send(command)).ToActionResult(this);

        [HttpGet("inbox")]
        public async Task<ActionResult<List<GetInboxQueryResponseItem>>> GetInbox([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
            => (await _mediator.Send(new GetInboxQuery(pageNumber ?? 1, pageSize ?? 5))).ToActionResult(this);

        [HttpGet("sendbox")]
        public async Task<ActionResult<List<GetInboxQueryResponseItem>>> GetSendbox([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
            => (await _mediator.Send(new GetSendboxQuery(pageNumber ?? 1, pageSize ?? 5))).ToActionResult(this);

        [HttpGet("inbox-count")]
        public async Task<ActionResult<GetInboxCountQueryResponse>> GetInboxCount()
            => (await _mediator.Send(new GetInboxCountQuery())).ToActionResult(this);

        [HttpGet("{id}")]
        public async Task<ActionResult<GetMessageQueryResponse>> GetMessage(Guid id)
            => (await _mediator.Send(new GetMessageQuery(id))).ToActionResult(this);

        [HttpGet("search-friends")]
        public async Task<ActionResult<List<SearchFriendsQueryResponseItem>>> SearchFriends([FromQuery] string prefix)
             => (await _mediator.Send(new SearchFriendsQuery(prefix))).ToActionResult(this);
    }
}
