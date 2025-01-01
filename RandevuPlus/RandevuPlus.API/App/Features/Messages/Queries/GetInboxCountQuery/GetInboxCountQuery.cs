using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxCountQuery
{
    public sealed record GetInboxCountQuery : IRequest<Result<GetInboxCountQueryResponse>>;
}
