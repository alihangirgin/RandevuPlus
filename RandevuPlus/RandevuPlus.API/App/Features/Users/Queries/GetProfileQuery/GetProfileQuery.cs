using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery
{
    public sealed record GetProfileQuery : IRequest<Result<GetProfileQueryResponse>>;

}
