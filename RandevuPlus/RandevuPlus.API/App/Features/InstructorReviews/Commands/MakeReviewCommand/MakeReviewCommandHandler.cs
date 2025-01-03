using Ardalis.Result;
using AutoMapper;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.InstructorReviews.Commands.MakeReviewCommand
{
    public class MakeReviewCommandHandler : IRequestHandler<MakeReviewCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MakeReviewCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> Handle(MakeReviewCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;

            var checkAppointment = await _unitOfWork.Appointments.CheckAppointmentAsync(userId, command.InstructorId);
            if (!checkAppointment) return Result.Error("Unauthorized");

            var instructorReview = _mapper.Map<InstructorReview>(command);
            instructorReview.UserId = userId;

            await _unitOfWork.InstructorReviews.AddAsync(instructorReview);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
