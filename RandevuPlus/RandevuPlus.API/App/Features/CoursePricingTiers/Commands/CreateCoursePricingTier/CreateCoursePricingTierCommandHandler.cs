using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.CoursePricingTiers.Commands.CreateCoursePricingTier
{
    public class CreateCoursePricingTierCommandHandler : IRequestHandler<CreateCoursePricingTierCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCoursePricingTierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateCoursePricingTierCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.CoursePricingTiers.DuplicateMinHourExistAsync(command.CourseId, command.MinHours))
                return Result.Error("DuplicateMinHour");

            if (await _unitOfWork.CoursePricingTiers.DuplicateMaxHourExistAsync(command.CourseId, command.MaxHours))
                return Result.Error("DuplicateMaxHour");

            var coursePricingTier = _mapper.Map<CoursePricingTier>(command);
            await _unitOfWork.CoursePricingTiers.AddAsync(coursePricingTier);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
