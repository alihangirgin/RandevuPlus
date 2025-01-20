using RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsFromAiQuery
{
    public sealed record GetInstructorsFromAiQueryResponse(string Text, List<GetInstructorsQueryResponse> Instructors);
}
