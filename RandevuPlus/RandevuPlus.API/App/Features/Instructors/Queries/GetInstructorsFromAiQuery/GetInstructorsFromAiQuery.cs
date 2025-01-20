using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsFromAiQuery
{
    public sealed record GetInstructorsFromAiQuery(string Question) : IRequest<Result<GetInstructorsFromAiQueryResponse>>;
}
