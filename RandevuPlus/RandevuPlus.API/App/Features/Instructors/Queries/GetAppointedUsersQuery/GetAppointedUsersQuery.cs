using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetAppointedUsersQuery
{
    public sealed record GetAppointedUsersQuery : IRequest<Result<List<GetAppointedUsersQueryResponse>>>;
}
