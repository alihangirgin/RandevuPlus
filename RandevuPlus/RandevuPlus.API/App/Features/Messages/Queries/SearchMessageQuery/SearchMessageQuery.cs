using Ardalis.Result;
using MediatR;
using RandevuPlus.API.App.Features.Messages.Queries.GetInboxQuery;

namespace RandevuPlus.API.App.Features.Messages.Queries.SearchMessageQuery
{
    public sealed record SearchMessageQuery(string Prefix) : IRequest<Result<List<GetInboxQueryResponseItem>>>;

}
