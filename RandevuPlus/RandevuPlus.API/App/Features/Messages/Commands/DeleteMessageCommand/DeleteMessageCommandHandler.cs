using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Commands.DeleteMessageCommand
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteMessageCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteMessageCommand command, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(command.Id);
            if (message == null) return Result.Error("MessageNotFound");

            var userId = _currentUserService.UserId.Value;
            if (message.SenderId != userId) return Result.Error("Unauthorized");

            if (message.SenderId == userId) message.IsRemovedFromSender = true;
            if (message.ReceiverId == userId) message.IsRemovedFromReceiver = true;

            await _unitOfWork.Messages.UpdateAsync(message);
            await _unitOfWork.CommitAsync();
            return Result.Success();
        }
    }
}
