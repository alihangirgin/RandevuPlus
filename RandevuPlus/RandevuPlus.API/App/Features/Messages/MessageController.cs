using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RandevuPlus.API.App.Features.Messages.Commands.AddMessageReactionCommand;
using RandevuPlus.API.App.Features.Messages.Commands.DeleteMessageCommand;
using RandevuPlus.API.App.Features.Messages.Commands.SendMessageCommand;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxCountQuery;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;
using RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery;
using RandevuPlus.API.App.Features.Messages.Queries.SearchFriendsQuery;
using RandevuPlus.API.App.Features.Messages.Queries.SearchMessageQuery;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.App.Features.Messages
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> DeleteMessage(Guid id)
            => await _mediator.Send(new DeleteMessageCommand(id));

        [HttpPost]
        public async Task<ActionResult<Result>> SendMessage([FromBody] SendMessageCommand command)
            => await _mediator.Send(command);

        [HttpPost("{messageId}/reactions")]
        public async Task<ActionResult<Result>> AddReactionToMessage(Guid messageId, [FromBody] AddMessageReactionCommand command)
         => await _mediator.Send(command.SetMessageId(messageId));

        [HttpGet("inbox")]
        public async Task<ActionResult<Result<PaginatedResponse<GetInboxQueryResponseItem>>>> GetInbox([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
            => await _mediator.Send(new GetInboxQuery(pageNumber ?? 1, pageSize ?? 5));

        [HttpGet("inbox-count")]
        public async Task<ActionResult<GetInboxCountQueryResponse>> GetInboxCount()
            => (await _mediator.Send(new GetInboxCountQuery())).ToActionResult(this);

        [HttpGet("{recipientId}")]
        public async Task<ActionResult<Result<GetMessageQueryResponse>>> GetMessage(Guid recipientId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
            => await _mediator.Send(new GetMessageQuery(recipientId, pageNumber ?? 1, pageSize ?? 5));

        [HttpGet("search")]
        public async Task<ActionResult<Result<List<GetInboxQueryResponseItem>>>> SearchMessage([FromQuery] string prefix)
            => await _mediator.Send(new SearchMessageQuery(prefix));

        [HttpGet("search-friends")]
        public async Task<ActionResult<Result<List<SearchFriendsQueryResponseItem>>>> SearchFriends([FromQuery] string prefix)
             => await _mediator.Send(new SearchFriendsQuery(prefix));
    }
}
