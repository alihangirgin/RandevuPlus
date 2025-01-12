using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RandevuPlus.API.Infrastructure.Services;
using RandevuPlus.API.Infrastructure.Sockets;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Commands.AddMessageReactionCommand
{
    public class AddMessageReactionCommandHandler : IRequestHandler<AddMessageReactionCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public AddMessageReactionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IHubContext<UserHub> hubContext, IUserService userService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _userService = userService;
        }

        public async Task<Result> Handle(AddMessageReactionCommand command, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(command.MessageId, include: "Reactions");
            if (message == null) return Result.Error("MessageNotFound");

            var userId = _currentUserService.UserId.Value;
            if (message.SenderId != userId && message.ReceiverId != userId) return Result.Error("Unauthorized");

            var existedReaction = message.Reactions.FirstOrDefault(x => x.ReactorId == userId && x.Reaction == command.Reaction);
            if (existedReaction != null)
                message.Reactions.Remove(existedReaction);
            else
            {
                message.Reactions.Add(new MessageReaction()
                {
                    ReactorId = userId,
                    Reaction = command.Reaction,
                    CreatedBy = "test"
                });
            }
            await _unitOfWork.Messages.UpdateAsync(message);
            await _unitOfWork.CommitAsync();

            var eventReceiverId = message.SenderId != userId ? message.SenderId : message.ReceiverId;
            if (_userService.GetOnlineUsers().Contains(eventReceiverId.ToString()))
                await _hubContext.Clients.User(eventReceiverId.ToString()).SendAsync("MessageUpdated", userId);
            return Result.Success();
        }
    }
}
