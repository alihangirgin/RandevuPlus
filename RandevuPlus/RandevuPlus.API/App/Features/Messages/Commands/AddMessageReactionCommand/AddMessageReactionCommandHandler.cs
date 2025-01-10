using Ardalis.Result;
using MediatR;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Messages.Commands.AddMessageReactionCommand
{
    public class AddMessageReactionCommandHandler : IRequestHandler<AddMessageReactionCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public AddMessageReactionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
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
            return Result.Success();
        }
    }
}
