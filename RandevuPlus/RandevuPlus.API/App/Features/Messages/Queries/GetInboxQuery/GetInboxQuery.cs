using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Dtos;

namespace RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery
{
    public sealed record GetInboxQuery(int PageNumber, int PageSize) : IRequest<Result<PaginatedResponse<GetInboxQueryResponseItem>>>;

}
