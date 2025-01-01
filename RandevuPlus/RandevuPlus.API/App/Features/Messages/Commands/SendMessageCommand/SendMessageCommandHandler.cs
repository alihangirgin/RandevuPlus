using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Commands.SendMessageCommand
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public SendMessageCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            var userExists = await _unitOfWork.Users.CheckAsync(command.ReceiverId);
            if (!userExists) return Result.Error("UserNotFound");

            var userId = _currentUserService.UserId.Value;
            var isInstructor = _currentUserService.Roles.Contains("Instructor");
            if (isInstructor)
            {
                var checkAppointment = await _unitOfWork.Appointments.CheckAppointmentAsync(command.ReceiverId, userId);
                if (!checkAppointment) return Result.Error("Unauthorized");
            }
            else
            {
                var checkAppointment = await _unitOfWork.Appointments.CheckAppointmentAsync(userId, command.ReceiverId);
                if (!checkAppointment) return Result.Error("Unauthorized");
            }

            var message = new Message()
            {
                SenderId = userId,
                ReceiverId = command.ReceiverId,
                Title = command.Title,
                MessageText = command.MessageText,
                IsRead = false
            };
            await _unitOfWork.Messages.AddAsync(message);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
