using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.App.Features.Instructors.Queries.GetInsructorQuery;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;
using System.Linq.Expressions;

namespace RandevuPlus.API.App.Features.Instructors.Queries.GetInstructorsQuery
{
    public class GetInstructorsQueryHandler : IRequestHandler<GetInstructorsQuery, Result<List<GetInstructorQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetInstructorsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetInstructorQueryResponse>>> Handle(GetInstructorsQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<Instructor, bool>>? filterQuery = null;
            if (query.InstructorId != null && string.IsNullOrEmpty(query.Prefix))
                filterQuery = x => x.Id == query.InstructorId;
            //else if (query.InstructorId != null && !string.IsNullOrEmpty(query.Prefix))
            //    filterQuery = x => x.Id == query.InstructorId && (x.Name.Contains(query.Prefix) || (x.Bio != null && x.Bio.Contains(query.Prefix)));
            //else if (query.InstructorId == null && !string.IsNullOrEmpty(query.Prefix))
            //    filterQuery = x => x.Name.Contains(query.Prefix) || (x.Bio != null && x.Bio.Contains(query.Prefix));

            var instructors = await _unitOfWork.Instructors.GetPaginatedAsync(query.PageNumber, query.PageSize, filter: filterQuery, orderBy: x => x.OrderBy(y => y.CreatedAt));
            var response = _mapper.Map<List<GetInstructorQueryResponse>>(instructors);
            return Result<List<GetInstructorQueryResponse>>.Success(response);
        }
    }
}
