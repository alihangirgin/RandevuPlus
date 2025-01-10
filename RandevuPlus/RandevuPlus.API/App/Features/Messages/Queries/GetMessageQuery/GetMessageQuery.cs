using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery
{
    public sealed record GetMessageQuery(Guid RecipientId, int PageNumber, int PageSize) : IRequest<Result<GetMessageQueryResponse>>;
}
