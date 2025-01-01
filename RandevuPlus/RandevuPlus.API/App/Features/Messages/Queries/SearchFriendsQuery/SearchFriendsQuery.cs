using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Messages.Queries.SearchFriendsQuery
{
    public sealed record SearchFriendsQuery(string Prefix) : IRequest<Result<List<SearchFriendsQueryResponseItem>>>;
}
