using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetSendboxQuery
{
    public sealed record GetSendboxQuery(int PageNumber, int PageSize) : IRequest<Result<List<GetInboxQueryResponseItem>>>;
}
