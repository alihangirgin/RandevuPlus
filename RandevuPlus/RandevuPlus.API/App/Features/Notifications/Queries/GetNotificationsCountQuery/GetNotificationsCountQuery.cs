using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Notifications.Queries.GetNotificationsCountQuery
{
    public sealed record GetNotificationsCountQuery : IRequest<Result<GetNotificationsCountQueryResponse>>;
}
