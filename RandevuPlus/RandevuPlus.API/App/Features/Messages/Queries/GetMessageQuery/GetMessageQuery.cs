using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetMessageQuery
{
    public sealed record GetMessageQuery(Guid Id) : IRequest<Result<GetMessageQueryResponse>>;
}
