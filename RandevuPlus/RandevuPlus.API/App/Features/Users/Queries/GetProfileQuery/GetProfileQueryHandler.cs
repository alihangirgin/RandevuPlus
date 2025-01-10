using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Services;
using RandevuPlus.API.Shared.Interfaces.UnitOfWork;

namespace RandevuPlus.API.App.Features.Users.Queries.GetProfileQuery
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<GetProfileQueryResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public GetProfileQueryHandler(UserManager<AppUser> userManager, ICurrentUserService currentUserService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetProfileQueryResponse>> Handle(GetProfileQuery query, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId.Value;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Result.Error("UserNotFound");
            var response = _mapper.Map<GetProfileQueryResponse>(user);
            var inboxCount = await _unitOfWork.Messages.CountInboxAsync(userId);
            response = response with { Roles = _currentUserService.Roles.ToArray(), InboxCount = inboxCount};
            return Result.Success(response);
        }
    }
}
