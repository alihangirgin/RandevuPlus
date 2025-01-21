using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Notifications.Queries.GetNotificationsQuery
{
    public sealed record GetNotificationsQuery : IRequest<Result<List<GetNotificationsQueryResponse>>>;
}
