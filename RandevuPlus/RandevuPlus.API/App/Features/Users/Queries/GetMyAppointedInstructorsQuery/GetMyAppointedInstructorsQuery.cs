using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Queries.GetMyAppointedInstructorsQuery
{
    public sealed record GetMyAppointedInstructorsQuery : IRequest<Result<List<GetMyAppointedInstructorsQueryResponse>>>;
}
