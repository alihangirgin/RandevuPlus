using Ardalis.Result;
using MediatR;

namespace RandevuPlus.API.App.Features.Users.Queries.GetSavedInstructorsQuery
{
    public class GetSavedInstructorsQuery() : IRequest<Result<List<GetSavedInstructorsQueryResponse>>>;
}
