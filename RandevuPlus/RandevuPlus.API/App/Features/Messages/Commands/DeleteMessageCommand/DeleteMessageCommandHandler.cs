using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RandevuPlus.API.Infrastructure.Sockets;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Commands.DeleteMessageCommand
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IUserService _userService;
        public DeleteMessageCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IHubContext<UserHub> hubContext, IUserService userService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _userService = userService;
        }

        public async Task<Result> Handle(DeleteMessageCommand command, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(command.Id);
            if (message == null) return Result.Error("MessageNotFound");

            var userId = _currentUserService.UserId.Value;
            if (message.SenderId != userId) return Result.Error("Unauthorized");

            message.IsRemovedFromSender = true;
            message.IsRemovedFromReceiver = true;

            await _unitOfWork.Messages.UpdateAsync(message);
            await _unitOfWork.CommitAsync();

            var eventReceiverId = message.SenderId != userId ? message.SenderId : message.ReceiverId;
            if (_userService.GetOnlineUsers().Contains(eventReceiverId.ToString()))
                await _hubContext.Clients.User(eventReceiverId.ToString()).SendAsync("MessageUpdated", userId);

            return Result.Success();
        }
    }
}
