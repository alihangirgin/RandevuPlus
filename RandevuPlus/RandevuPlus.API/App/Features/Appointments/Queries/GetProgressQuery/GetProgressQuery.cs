using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Appointments.Queries.GetProgressQuery
{
    public sealed record GetProgressQuery() : IRequest<Result<GetProgressQueryResponse>>;
}
