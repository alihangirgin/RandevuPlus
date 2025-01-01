using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery
{
    public sealed record GetInboxQuery(int PageNumber, int PageSize) : IRequest<Result<List<GetInboxQueryResponseItem>>>;

}
